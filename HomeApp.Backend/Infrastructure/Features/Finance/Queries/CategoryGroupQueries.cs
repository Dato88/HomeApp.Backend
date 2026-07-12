using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Queries;

public sealed class CategoryGroupQueries(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : ICategoryGroupQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<List<CategoryGroup>>> GetCategoryGroupsAsync(int householdId,
        CancellationToken cancellationToken)
    {
        var categoryGroups = await _dbContext.CategoryGroups
            .AsNoTracking()
            .Where(g =>
                g.HouseholdId == householdId &&
                g.Household.Members.Any(m => m.PersonId == _executionContext.PersonId))
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);

        return Result.Success(categoryGroups);
    }
}
