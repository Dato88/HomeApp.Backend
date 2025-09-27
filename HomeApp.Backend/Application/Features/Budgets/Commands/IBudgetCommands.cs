using Domain.Entities.Budgets;
using SharedKernel;

namespace Application.Features.Budgets.Commands;

public interface IBudgetCommands
{
    Task<Result<int>> CreateBudgetAsync(int year, CancellationToken cancellationToken);
    Task<Result<int>> CreateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);
    Task<Result<int>> CreateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken);
}
