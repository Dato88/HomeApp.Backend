using Application.Abstractions.Authentication;
using Application.Abstractions.HouseholdModule;
using Domain.Entities.Households;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Features.Households.Queries;

public sealed class HouseholdQueries(
    HomeAppContext dbContext,
    IExecutionContextAccessor executionContext) : IHouseholdQueries
{
    private readonly HomeAppContext _dbContext = dbContext;
    private readonly IExecutionContextAccessor _executionContext = executionContext;

    public async Task<Result<List<Household>>> GetHouseholdsAsync(CancellationToken cancellationToken)
    {
        var households = await _dbContext.Households
            .AsNoTracking()
            .Include(h => h.Members)
            .ThenInclude(m => m.Person)
            .Where(h => h.Members.Any(m => m.PersonId == _executionContext.PersonId))
            .ToListAsync(cancellationToken);

        return Result.Success(households);
    }
}
