using Domain.Entities.Budgets;
using SharedKernel;

namespace Application.Features.Budgets.Queries;

public interface IBudgetQueries
{
    Task<Result<Budget>> GetBudgetAsync(int year, CancellationToken cancellationToken);
}
