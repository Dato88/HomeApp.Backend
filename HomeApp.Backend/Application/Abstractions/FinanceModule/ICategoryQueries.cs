using Domain.Entities.Finance;
using SharedKernel;

namespace Application.Abstractions.FinanceModule;

public interface ICategoryQueries
{
    Task<Result<List<Category>>> GetCategoriesAsync(int householdId, CancellationToken cancellationToken);
}
