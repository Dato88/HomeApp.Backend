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

                CreateLogBudgetCellInformation(budgetCell, DateTime.Now);
            }
            catch
            {
                CreateLogBudgetCellError(budgetCell, DateTime.Now);
            }
        }

        public async Task CreateBudgetColumnAsync(BudgetColumn budgetColumn)
        {
            try
            {
                await _budgetColumnCrud.CreateAsync(budgetColumn);

                CreateLogBudgetColumnInformation(budgetColumn, DateTime.Now);
            }
            catch
            {
                CreateLogBudgetColumnError(budgetColumn, DateTime.Now);
            }
        }

        public async Task CreateBudgetGroupAsync(BudgetGroup budgetGroup)
        {
            try
            {
                await _budgetGroupCrud.CreateAsync(budgetGroup);

                CreateLogBudgetGroupInformation(budgetGroup, DateTime.Now);
            }
            catch
            {
                CreateLogBudgetGroupError(budgetGroup, DateTime.Now);
            }
        }

        public async Task CreateBudgetRowAsync(BudgetRow budgetRow)
        {
            try
            {
                await _budgetRowCrud.CreateAsync(budgetRow);

                CreateLogBudgetRowInformation(budgetRow, DateTime.Now);
            }
            catch
            {
                CreateLogBudgetRowError(budgetRow, DateTime.Now);
            }
        }

        public async Task UpdateBudgetCellAsync(BudgetCell budgetCell)
        {
            try
            {
                await _budgetCellCrud.UpdateAsync(budgetCell);

                UpdateLogBudgetCellInformation(budgetCell, DateTime.Now);
            }
            catch
            {
                UpdateLogBudgetCellError(budgetCell, DateTime.Now);
            }
        }

        public async Task UpdateBudgetColumnAsync(BudgetColumn budgetColumn)
        {
            try
            {
                await _budgetColumnCrud.UpdateAsync(budgetColumn);

                UpdateLogBudgetColumnInformation(budgetColumn, DateTime.Now);
            }
            catch
            {
                UpdateLogBudgetColumnError(budgetColumn, DateTime.Now);
            }
        }

        public async Task UpdateBudgetGroupAsync(BudgetGroup budgetGroup)
        {
            try
            {
                await _budgetGroupCrud.UpdateAsync(budgetGroup);

                UpdateLogBudgetGroupInformation(budgetGroup, DateTime.Now);
            }
            catch
            {
                UpdateLogBudgetGroupError(budgetGroup, DateTime.Now);
            }
        }

        public async Task UpdateBudgetRowAsync(BudgetRow budgetRow)
        {
            try
            {
                await _budgetRowCrud.UpdateAsync(budgetRow);

                UpdateLogBudgetRowInformation(budgetRow, DateTime.Now);
            }
            catch
            {
                UpdateLogBudgetRowError(budgetRow, DateTime.Now);
            }
        }


        public async Task DeleteBudgetCellAsync(int id)
        {
            try
            {
                await _budgetCellCrud.DeleteAsync(id);

                DeleteLogBudgetCellInformation(id, DateTime.Now);
            }
            catch
            {
                DeleteLogBudgetCellError(id, DateTime.Now);
            }
        }

        public async Task DeleteBudgetColumnAsync(int id)
        {
            try
            {
                await _budgetColumnCrud.DeleteAsync(id);

                DeleteLogBudgetColumnInformation(id, DateTime.Now);
            }
            catch
            {
                DeleteLogBudgetColumnError(id, DateTime.Now);
            }
        }

        public async Task DeleteBudgetGroupAsync(int id)
        {
            try
            {
                await _budgetGroupCrud.DeleteAsync(id);

                DeleteLogBudgetGroupInformation(id, DateTime.Now);
            }
            catch
            {
                DeleteLogBudgetGroupError(id, DateTime.Now);
            }
        }

        public async Task DeleteBudgetRowAsync(int id)
        {
            try
            {
                await _budgetRowCrud.DeleteAsync(id);

                DeleteLogBudgetRowInformation(id, DateTime.Now);
            }
            catch
            {
                DeleteLogBudgetRowError(id, DateTime.Now);
            }
        }
    }
}
