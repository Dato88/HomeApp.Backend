using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;
using SharedKernel;

namespace Application.Abstractions.BudgetModule;

public interface IBudgetCommands
{
    Task<Result<int>> CreateBudgetAsync(int householdId, int year, CancellationToken cancellationToken);
    Task<Result<int>> CreateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);
    Task<Result<int>> CreateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken);
    Task<Result<int>> CreateBudgetCellAsync(BudgetCell budgetCell, CancellationToken cancellationToken);

    Task<Result<int>> DeleteBudgetAsync(int budgetId, CancellationToken cancellationToken);
    Task<Result<int>> DeleteBudgetGroupAsync(int budgetGroupId, CancellationToken cancellationToken);
    Task<Result<int>> DeleteBudgetRowAsync(int budgetRowId, CancellationToken cancellationToken);
    Task<Result<int>> DeleteBudgetCellAsync(int budgetCellId, CancellationToken cancellationToken);

    Task<Result<int>> UpdateBudgetAsync(int budgetId, int year, CancellationToken cancellationToken);

    Task<Result<int>> UpdateBudgetGroupAsync(int budgetGroupId, int index, string title,
        BudgetGroupType budgetGroupType, decimal? targetPercent, CancellationToken cancellationToken);

    Task<Result<int>> UpdateBudgetRowAsync(int budgetRowId, int index, string title, int? categoryId,
        CancellationToken cancellationToken);

    Task<Result<int>> UpdateBudgetCellAsync(int budgetCellId, decimal amount, CancellationToken cancellationToken);
}
