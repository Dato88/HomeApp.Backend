using Application.Abstractions.Authentication;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Finance.Queries;

public sealed class CategoryQueries(HomeAppContext dbContext, IExecutionContextAccessor executionContext)
    : ICategoryQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<List<Category>>> GetCategoriesAsync(int householdId,
        CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories
            .AsNoTracking()
            .Where(c =>
                c.HouseholdId == householdId &&
                c.Household.Members.Any(m => m.PersonId == _executionContext.PersonId))
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return Result.Success(categories);
    }
}
