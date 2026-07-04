using Domain.Entities.Budgets;
using SharedKernel;

namespace Application.Abstractions.BudgetModule;

public interface IBudgetQueries
{
    Task<Result<Budget>> GetBudgetAsync(int householdId, int year, CancellationToken cancellationToken);
}
