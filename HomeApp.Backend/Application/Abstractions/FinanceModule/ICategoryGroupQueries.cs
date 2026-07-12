using Domain.Entities.Finance;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ICategoryGroupQueries
{
    Task<Result<List<CategoryGroup>>> GetCategoryGroupsAsync(int householdId, CancellationToken cancellationToken);
}
