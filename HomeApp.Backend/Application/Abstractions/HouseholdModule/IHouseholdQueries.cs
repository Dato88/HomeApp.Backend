using Domain.Entities.Households;
using SharedKernel;

namespace Application.Abstractions.HouseholdModule;

public interface IHouseholdQueries
{
    Task<Result<List<Household>>> GetHouseholdsAsync(CancellationToken cancellationToken);
}
