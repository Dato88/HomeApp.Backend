using Application.Abstractions.Authentication;
using Application.Features.Budgets.Queries;
using Domain.Entities.Budgets;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Budgets.Queries;

public sealed class BudgetQueries(
    HomeAppContext dbContext,
    IUserContext userContext) : IBudgetQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<Budget>> GetBudgetAsync(int year, CancellationToken cancellationToken)
    {
        var query = _dbContext.Budgets.AsQueryable().AsNoTracking();

        var budget = await query.Include(i => i.BudgetGroups)
            .ThenInclude(th => th.BudgetRows)
            .ThenInclude(th => th.BudgetCells)
            .AsSplitQuery()
            .SingleOrDefaultAsync(x => x.PersonId == _userContext.PersonId && x.Year == year, cancellationToken);

        if (budget is null)
            return Result.Failure<Budget>(BudgetErrors.NotFoundAll);

        return Result.Success(budget);
    }
}
