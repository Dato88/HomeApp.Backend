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
    }
}