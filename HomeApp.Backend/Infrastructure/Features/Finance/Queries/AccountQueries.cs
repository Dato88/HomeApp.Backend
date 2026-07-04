using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Queries;

public sealed class AccountQueries(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : IAccountQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<List<Account>>> GetAccountsAsync(CancellationToken cancellationToken)
    {
        var accounts = await _dbContext.Accounts
            .AsNoTracking()
            .Include(a => a.AccountHouseholds)
            .Where(a =>
                a.PersonId == _executionContext.PersonId ||
                a.AccountHouseholds.Any(ah =>
                    ah.Household.Members.Any(m => m.PersonId == _executionContext.PersonId)))
            .ToListAsync(cancellationToken);

        return Result.Success(accounts);
    }
}
