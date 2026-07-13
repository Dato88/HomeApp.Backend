using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Queries;

public sealed class PaymentPartnerQueries(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : IPaymentPartnerQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<List<PaymentPartnerWithStats>>> GetPaymentPartnersAsync(
        CancellationToken cancellationToken)
    {
        var items = await _dbContext.PaymentPartners
            .AsNoTracking()
            .Where(p => p.PersonId == _executionContext.PersonId)
            .OrderBy(p => p.DisplayName)
            .ThenBy(p => p.PaymentPartnerId)
            .Select(p => new PaymentPartnerWithStats(
                p,
                p.Transactions.Count,
                p.Transactions.Max(t => (DateOnly?)t.BookingDate)))
            .ToListAsync(cancellationToken);

        return Result.Success(items);
    }
}
