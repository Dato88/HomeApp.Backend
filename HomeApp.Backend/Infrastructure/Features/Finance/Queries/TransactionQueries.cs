using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Queries;

public sealed class TransactionQueries(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : ITransactionQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<TransactionPage>> GetTransactionsAsync(int accountId, DateOnly? from, DateOnly? to,
        int? categoryId, bool? uncategorized, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _dbContext.Transactions
            .AsNoTracking()
            .Where(t =>
                t.AccountId == accountId &&
                (t.Account.PersonId == _executionContext.PersonId ||
                 t.Account.AccountHouseholds.Any(ah =>
                     ah.Household.Members.Any(m => m.PersonId == _executionContext.PersonId))));

        if (from.HasValue)
            query = query.Where(t => t.BookingDate >= from.Value);

        if (to.HasValue)
            query = query.Where(t => t.BookingDate <= to.Value);

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (uncategorized == true)
            query = query.Where(t => t.CategoryId == null);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(t => t.BookingDate)
            .ThenByDescending(t => t.TransactionId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return Result.Success(new TransactionPage(totalCount, items));
    }
}
