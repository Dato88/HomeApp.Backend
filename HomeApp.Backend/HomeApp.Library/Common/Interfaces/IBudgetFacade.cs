using HomeApp.Library.Models;

namespace HomeApp.Library.Common.Interfaces;

public interface IBudgetFacade
{
    Task<Budget?> GetBudgetAsync(CancellationToken cancellationToken);
    Task CreateBudgetCellAsync(BudgetCell budgetCell, CancellationToken cancellationToken);
    Task CreateBudgetColumnAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken);
    Task CreateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);
    Task CreateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken);

    Task UpdateBudgetCellAsync(BudgetCell budgetCell, CancellationToken cancellationToken);
    Task UpdateBudgetColumnAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken);
    Task UpdateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken);
    Task UpdateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken);

    Task DeleteBudgetCellAsync(int id, CancellationToken cancellationToken);
    Task DeleteBudgetColumnAsync(int id, CancellationToken cancellationToken);
    Task DeleteBudgetGroupAsync(int id, CancellationToken cancellationToken);
    Task DeleteBudgetRowAsync(int id, CancellationToken cancellationToken);
}
