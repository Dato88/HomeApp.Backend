using HomeApp.Library.Models;

namespace HomeApp.Library.Facades.Interfaces
{
    public interface IBudgetFacade
    {
        Task<Budget> GetBudgetAsync(int userId);
        Task PostBudgetCellAsync(BudgetCell budgetCell);
        Task PostBudgetColumnAsync(BudgetColumn budgetColumn);
        Task PostBudgetGroupAsync(BudgetGroup budgetGroup);
        Task PostBudgetRowAsync(BudgetRow budgetRow);
    }
}