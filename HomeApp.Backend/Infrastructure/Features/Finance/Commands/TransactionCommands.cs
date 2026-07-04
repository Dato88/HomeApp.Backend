using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Commands;

public sealed class TransactionCommands(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : ITransactionCommands
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<int>> CreateTransactionAsync(Transaction transaction,
        CancellationToken cancellationToken)
    {
        var accountIsOwned = _dbContext.Accounts.Any(a =>
            a.AccountId == transaction.AccountId && a.PersonId == _executionContext.PersonId);

        if (!accountIsOwned)
            return Result.Failure<int>(FinanceErrors.TransactionCreateFailedWithMessage("AccountId is invalid"));

        var categoryError = ValidateTransactionCategory(transaction.AccountId, transaction.CategoryId);

        if (categoryError is not null)
            return Result.Failure<int>(FinanceErrors.TransactionCreateFailedWithMessage(categoryError));

        transaction.CreatedById = _executionContext.PersonId;

        _dbContext.Transactions.Add(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(transaction.TransactionId);
    }

    public async Task<Result<int>> UpdateTransactionAsync(Transaction transaction,
        CancellationToken cancellationToken)
    {
        var existing = await _dbContext.Transactions.SingleOrDefaultAsync(t =>
            t.TransactionId == transaction.TransactionId &&
            t.Account.PersonId == _executionContext.PersonId, cancellationToken);

        if (existing == null)
            return Result.Failure<int>(
                FinanceErrors.TransactionUpdateFailedWithMessage("TransactionId is invalid"));

        existing.BookingDate = transaction.BookingDate;
        existing.ValueDate = transaction.ValueDate;
        existing.Amount = transaction.Amount;
        existing.CounterpartyName = transaction.CounterpartyName;
        existing.CounterpartyIban = transaction.CounterpartyIban;
        existing.Purpose = transaction.Purpose;
        existing.UpdatedById = _executionContext.PersonId;
        existing.UpdatedAt = DateTime.UtcNow;

        _dbContext.Transactions.Update(existing);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(existing.TransactionId);
    }

    public async Task<Result<int>> DeleteTransactionAsync(int transactionId, CancellationToken cancellationToken)
    {
        var transaction = await _dbContext.Transactions.SingleOrDefaultAsync(t =>
            t.TransactionId == transactionId &&
            t.Account.PersonId == _executionContext.PersonId, cancellationToken);

        if (transaction == null)
            return Result.Failure<int>(
                FinanceErrors.TransactionDeleteFailedWithMessage("TransactionId is invalid"));

        _dbContext.Transactions.Remove(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(transactionId);
    }

    public async Task<Result<int>> SetTransactionCategoryAsync(int transactionId, int? categoryId,
        CancellationToken cancellationToken)
    {
        // Categorizing is allowed for the owner and for members of households the account is shared into
        var transaction = await _dbContext.Transactions.SingleOrDefaultAsync(t =>
            t.TransactionId == transactionId &&
            (t.Account.PersonId == _executionContext.PersonId ||
             t.Account.AccountHouseholds.Any(ah =>
                 ah.Household.Members.Any(m => m.PersonId == _executionContext.PersonId))), cancellationToken);

        if (transaction == null)
            return Result.Failure<int>(
                FinanceErrors.TransactionUpdateFailedWithMessage("TransactionId is invalid"));

        var categoryError = ValidateTransactionCategory(transaction.AccountId, categoryId);

        if (categoryError is not null)
            return Result.Failure<int>(FinanceErrors.TransactionUpdateFailedWithMessage(categoryError));

        transaction.CategoryId = categoryId;
        transaction.UpdatedById = _executionContext.PersonId;
        transaction.UpdatedAt = DateTime.UtcNow;

        _dbContext.Transactions.Update(transaction);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(transaction.TransactionId);
    }

    // Returns an error message or null. A transaction category must belong to a household the account is
    // shared into and the caller must be a member of that household, otherwise the E+A of that household
    // could be manipulated by outsiders or reference invisible data.
    private string? ValidateTransactionCategory(int accountId, int? categoryId)
    {
        if (!categoryId.HasValue)
            return null;

        var category = _dbContext.Categories
            .AsNoTracking()
            .SingleOrDefault(c => c.CategoryId == categoryId.Value);

        if (category == null)
            return "CategoryId is invalid";

        var callerIsMember = _dbContext.HouseholdMembers.Any(m =>
            m.HouseholdId == category.HouseholdId && m.PersonId == _executionContext.PersonId);

        if (!callerIsMember)
            return "CategoryId is invalid";

        var accountIsShared = _dbContext.AccountHouseholds.Any(ah =>
            ah.AccountId == accountId && ah.HouseholdId == category.HouseholdId);

        if (!accountIsShared)
            return "Account is not shared into the category's household";

        return null;
    }
}
