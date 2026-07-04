using Application.Abstractions.Authentication;
using Application.Abstractions.BudgetModule;
using Domain.Entities.Budgets;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Budgets.Queries;

public sealed class BudgetQueries(
    HomeAppContext dbContext,
    IExecutionContextAccessor executionContext) : IBudgetQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<Budget>> GetBudgetAsync(int householdId, int year, CancellationToken cancellationToken)
    {
        var query = _dbContext.Budgets.AsQueryable().AsNoTracking();

        var budget = await query.Include(i => i.BudgetGroups)
            .ThenInclude(th => th.BudgetRows)
            .ThenInclude(th => th.BudgetCells)
            .Include(i => i.BudgetGroups)
            .ThenInclude(th => th.BudgetRows)
            .ThenInclude(th => th.Category)
            .AsSplitQuery()
            .SingleOrDefaultAsync(x =>
                x.HouseholdId == householdId &&
                x.Year == year &&
                x.Household.Members.Any(m => m.PersonId == _executionContext.PersonId), cancellationToken);

        if (budget is null)
            return Result.Failure<Budget>(BudgetErrors.NotFoundAll);

        return Result.Success(budget);
    }
}
