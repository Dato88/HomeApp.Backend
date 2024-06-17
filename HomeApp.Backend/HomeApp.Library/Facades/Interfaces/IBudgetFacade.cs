using HomeApp.Library.Models;

namespace HomeApp.Library.Facades.Interfaces
{
    public interface IBudgetFacade
    {
        Task<Budget?> GetBudgetAsync(int userId);
        Task CreateBudgetCellAsync(BudgetCell budgetCell);
        Task CreateBudgetColumnAsync(BudgetColumn budgetColumn);
        Task CreateBudgetGroupAsync(BudgetGroup budgetGroup);
        Task CreateBudgetRowAsync(BudgetRow budgetRow);

        Task UpdateBudgetCellAsync(BudgetCell budgetCell);
        Task UpdateBudgetColumnAsync(BudgetColumn budgetColumn);
        Task UpdateBudgetGroupAsync(BudgetGroup budgetGroup);
        Task UpdateBudgetRowAsync(BudgetRow budgetRow);

        Task DeleteBudgetCellAsync(int id);
        Task DeleteBudgetColumnAsync(int id);
        Task DeleteBudgetGroupAsync(int id);
        Task DeleteBudgetRowAsync(int id);
    }
}