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
                LogException($"Get budget failed: {ex}", DateTime.Now);

                return null;
            }
        }

        public async Task CreateBudgetCellAsync(BudgetCell budgetCell)
        {
            try
            {
                await _budgetCellCrud.CreateAsync(budgetCell);

                LogInformation($"Creating budgetCell: {budgetCell}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Creating budgetCell failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task CreateBudgetColumnAsync(BudgetColumn budgetColumn)
        {
            try
            {
                await _budgetColumnCrud.CreateAsync(budgetColumn);

                LogInformation($"Creating budgetColumn: {budgetColumn}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Creating budgetColumn failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task CreateBudgetGroupAsync(BudgetGroup budgetGroup)
        {
            try
            {
                await _budgetGroupCrud.CreateAsync(budgetGroup);

                LogInformation($"Creating budgetGroup: {budgetGroup}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Creating budgetGroup failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task CreateBudgetRowAsync(BudgetRow budgetRow)
        {
            try
            {
                await _budgetRowCrud.CreateAsync(budgetRow);

                LogInformation($"Creating budgetRow: {budgetRow}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Creating budgetRow failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task UpdateBudgetCellAsync(BudgetCell budgetCell)
        {
            try
            {
                await _budgetCellCrud.UpdateAsync(budgetCell);

                LogInformation($"Updating budgetCell: {budgetCell}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Updating budgetCell failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task UpdateBudgetColumnAsync(BudgetColumn budgetColumn)
        {
            try
            {
                await _budgetColumnCrud.UpdateAsync(budgetColumn);

                LogInformation($"Updating budgetColumn: {budgetColumn}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Updating budgetColumn failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task UpdateBudgetGroupAsync(BudgetGroup budgetGroup)
        {
            try
            {
                await _budgetGroupCrud.UpdateAsync(budgetGroup);

                LogInformation($"Updating budgetGroup: {budgetGroup}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Updating budgetGroup failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task UpdateBudgetRowAsync(BudgetRow budgetRow)
        {
            try
            {
                await _budgetRowCrud.UpdateAsync(budgetRow);

                LogInformation($"Updating budgetRow: {budgetRow}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Updating budgetRow failed: {ex.Message}", DateTime.Now);
            }
        }


        public async Task DeleteBudgetCellAsync(int id)
        {
            try
            {
                await _budgetCellCrud.DeleteAsync(id);

                LogInformation($"Deleting budgetCell: {id}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Deleting budgetCell failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task DeleteBudgetColumnAsync(int id)
        {
            try
            {
                await _budgetColumnCrud.DeleteAsync(id);

                LogInformation($"Deleting budgetColumn: {id}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Deleting budgetColumn failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task DeleteBudgetGroupAsync(int id)
        {
            try
            {
                await _budgetGroupCrud.DeleteAsync(id);

                LogInformation($"Deleting budgetGroup: {id}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Deleting budgetGroup failed: {ex.Message}", DateTime.Now);
            }
        }

        public async Task DeleteBudgetRowAsync(int id)
        {
            try
            {
                await _budgetRowCrud.DeleteAsync(id);

                LogInformation($"Deleting budgetRow: {id}", DateTime.Now);
            }
            catch (Exception ex)
            {
                LogError($"Deleting budgetRow failed: {ex.Message}", DateTime.Now);
            }
        }
    }
}
