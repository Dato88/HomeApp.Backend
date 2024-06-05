using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models;

namespace HomeApp.Library.Facades
{
    public class BudgetFacade(IBudgetCellCrud budgetCellCrud, IBudgetColumnCrud budgetColumnCrud, IBudgetGroupCrud budgetGroupCrud, IBudgetRowCrud budgetRowCrud) : IBudgetFacade
    {
        private readonly IBudgetCellCrud _budgetCellCrud = budgetCellCrud;
        private readonly IBudgetColumnCrud _budgetColumnCrud = budgetColumnCrud;
        private readonly IBudgetGroupCrud _budgetGroupCrud = budgetGroupCrud;
        private readonly IBudgetRowCrud _budgetRowCrud = budgetRowCrud;

        public async Task<Budget> GetBudgetAsync(int userId)
        {
            Budget selectedBudget = new()
            {
                BudgetCells = await _budgetCellCrud.GetAllAsync(userId),
                BudgetColumns = await _budgetColumnCrud.GetAllAsync(),
                BudgetGroups = await _budgetGroupCrud.GetAllAsync(userId),
                BudgetRows = await _budgetRowCrud.GetAllAsync(userId),
            };

            return selectedBudget;
        }
    }
}
