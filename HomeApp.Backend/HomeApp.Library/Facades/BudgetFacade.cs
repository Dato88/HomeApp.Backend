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

        public async Task PostBudgetCellAsync(BudgetCell budgetCell)
        {
            try
            {
                await _budgetCellCrud.CreateAsync(budgetCell);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task PostBudgetColumnAsync(BudgetColumn budgetColumn)
        {
            try
            {
                await _budgetColumnCrud.CreateAsync(budgetColumn);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task PostBudgetGroupAsync(BudgetGroup budgetGroup)
        {
            try
            {
                await _budgetGroupCrud.CreateAsync(budgetGroup);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task PostBudgetRowAsync(BudgetRow budgetRow)
        {
            try
            {
                await _budgetRowCrud.CreateAsync(budgetRow);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
