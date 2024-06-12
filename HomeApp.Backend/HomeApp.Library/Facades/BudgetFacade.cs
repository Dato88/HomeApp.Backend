using HomeApp.DataAccess.Models;
using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using HomeApp.Library.Models;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Facades
{
    public partial class BudgetFacade(IBudgetCellCrud budgetCellCrud, IBudgetColumnCrud budgetColumnCrud, IBudgetGroupCrud budgetGroupCrud, IBudgetRowCrud budgetRowCrud, ILogger<BudgetFacade> logger) : BudgetLoggerExtension<BudgetFacade>(logger), IBudgetFacade
    {
        private readonly IBudgetCellCrud _budgetCellCrud = budgetCellCrud;
        private readonly IBudgetColumnCrud _budgetColumnCrud = budgetColumnCrud;
        private readonly IBudgetGroupCrud _budgetGroupCrud = budgetGroupCrud;
        private readonly IBudgetRowCrud _budgetRowCrud = budgetRowCrud;

        public async Task<Budget?> GetBudgetAsync(int userId)
        {
            try
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
            catch (Exception ex)
            {
                LogBudgetError(ex, DateTime.Now);

                return null;
            }
        }

        public async Task CreateBudgetCellAsync(BudgetCell budgetCell)
        {
            try
            {
                await _budgetCellCrud.CreateAsync(budgetCell);

                LogBudgetCell(budgetCell, DateTime.Now);
            }
            catch
            {
                LogBudgetCellError(budgetCell, DateTime.Now);
            }
        }

        public async Task CreateBudgetColumnAsync(BudgetColumn budgetColumn)
        {
            try
            {
                await _budgetColumnCrud.CreateAsync(budgetColumn);

                LogBudgetColumn(budgetColumn, DateTime.Now);
            }
            catch
            {
                LogBudgetColumnError(budgetColumn, DateTime.Now);
            }
        }

        public async Task CreateBudgetGroupAsync(BudgetGroup budgetGroup)
        {
            try
            {
                await _budgetGroupCrud.CreateAsync(budgetGroup);

                LogBudgetGroup(budgetGroup, DateTime.Now);
            }
            catch
            {
                LogBudgetGroupError(budgetGroup, DateTime.Now);
            }
        }

        public async Task CreateBudgetRowAsync(BudgetRow budgetRow)
        {
            try
            {
                await _budgetRowCrud.CreateAsync(budgetRow);

                LogBudgetRow(budgetRow, DateTime.Now);
            }
            catch
            {
                LogBudgetRowError(budgetRow, DateTime.Now);
            }
        }
    }
}
