using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Commands;

public sealed class PaymentPartnerCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : IPaymentPartnerCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> RenamePaymentPartnerAsync(int paymentPartnerId, string displayName,
        CancellationToken cancellationToken)
    {
        var partner = await _dbContext.PaymentPartners.SingleOrDefaultAsync(p =>
            p.PaymentPartnerId == paymentPartnerId &&
            p.PersonId == _executionContext.PersonId, cancellationToken);

        if (partner == null)
            return Result.Failure<int>(
                FinanceErrors.PaymentPartnerUpdateFailedWithMessage("PaymentPartnerId is invalid"));

        // Rename is cosmetic: NormalizedName stays as the matching key so future imports keep matching
        partner.DisplayName = displayName.Trim();
        partner.UpdatedById = _executionContext.PersonId;
        partner.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(partner.PaymentPartnerId);
    }

    public async Task<Result<int>> MergePaymentPartnersAsync(int targetPaymentPartnerId,
        int sourcePaymentPartnerId, CancellationToken cancellationToken)
    {
        if (targetPaymentPartnerId == sourcePaymentPartnerId)
            return Result.Failure<int>(FinanceErrors.PaymentPartnerMergeFailedWithMessage(
                "Cannot merge a payment partner into itself"));

        var partners = await _dbContext.PaymentPartners
            .Where(p =>
                (p.PaymentPartnerId == targetPaymentPartnerId || p.PaymentPartnerId == sourcePaymentPartnerId) &&
                p.PersonId == _executionContext.PersonId)
            .ToListAsync(cancellationToken);

        var target = partners.SingleOrDefault(p => p.PaymentPartnerId == targetPaymentPartnerId);
        var source = partners.SingleOrDefault(p => p.PaymentPartnerId == sourcePaymentPartnerId);

        if (target == null || source == null)
            return Result.Failure<int>(
                FinanceErrors.PaymentPartnerMergeFailedWithMessage("PaymentPartnerId is invalid"));

        await using var dbTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        var moved = await _dbContext.Transactions
            .Where(t => t.PaymentPartnerId == sourcePaymentPartnerId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(t => t.PaymentPartnerId, targetPaymentPartnerId)
                .SetProperty(t => t.UpdatedById, _executionContext.PersonId)
                .SetProperty(t => t.UpdatedAt, DateTime.UtcNow), cancellationToken);

        // A target without an IBAN inherits the source's IBAN (and its own-account link):
        // the IBAN is the strongest matching key and would otherwise be lost with the source.
        var inheritIban = target.Iban is null && source.Iban is not null;
        var inheritedIban = source.Iban;
        var inheritedLinkedAccountId = source.LinkedAccountId;

        // Delete first: frees the unique (person_id, iban) slot within the transaction
        _dbContext.PaymentPartners.Remove(source);
        await _dbContext.SaveChangesAsync(cancellationToken);

        if (inheritIban)
        {
            target.Iban = inheritedIban;
            target.LinkedAccountId = inheritedLinkedAccountId;
            target.UpdatedById = _executionContext.PersonId;
            target.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        await dbTransaction.CommitAsync(cancellationToken);

        return Result.Success(moved);
    }
}
