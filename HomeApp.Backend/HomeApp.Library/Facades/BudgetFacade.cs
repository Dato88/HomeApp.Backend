using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using HomeApp.Library.Models;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Facades;

public class BudgetFacade(
    IBudgetCellCrud budgetCellCrud,
    IBudgetColumnCrud budgetColumnCrud,
    IBudgetGroupCrud budgetGroupCrud,
    IBudgetRowCrud budgetRowCrud,
    IPersonFacade personFacade,
    ILogger<BudgetFacade> logger) : BudgetLoggerExtension<BudgetFacade>(logger), IBudgetFacade
{
    private readonly IBudgetCellCrud _budgetCellCrud = budgetCellCrud;
    private readonly IBudgetColumnCrud _budgetColumnCrud = budgetColumnCrud;
    private readonly IBudgetGroupCrud _budgetGroupCrud = budgetGroupCrud;
    private readonly IBudgetRowCrud _budgetRowCrud = budgetRowCrud;
    private readonly IPersonFacade _personFacade = personFacade;

    public async Task<Budget?> GetBudgetAsync(CancellationToken cancellationToken)
    {
        try
        {
            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

            Budget selectedBudget = new()
            {
                BudgetCells = await _budgetCellCrud.GetAllAsync(person.Id, cancellationToken),
                BudgetColumns = await _budgetColumnCrud.GetAllAsync(cancellationToken),
                BudgetGroups = await _budgetGroupCrud.GetAllAsync(person.Id, cancellationToken),
                BudgetRows = await _budgetRowCrud.GetAllAsync(person.Id, cancellationToken)
            };

            return selectedBudget;
        }
        catch (Exception ex)
        {
            LogException($"Get budget failed: {ex}", DateTime.Now);

            return null;
        }
    }

    public async Task CreateBudgetCellAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetCellCrud.CreateAsync(budgetCell, cancellationToken);

            LogInformation($"Creating budgetCell: {budgetCell}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Creating budgetCell failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task CreateBudgetColumnAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetColumnCrud.CreateAsync(budgetColumn, cancellationToken);

            LogInformation($"Creating budgetColumn: {budgetColumn}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Creating budgetColumn failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task CreateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetGroupCrud.CreateAsync(budgetGroup, cancellationToken);

            LogInformation($"Creating budgetGroup: {budgetGroup}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Creating budgetGroup failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task CreateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetRowCrud.CreateAsync(budgetRow, cancellationToken);

            LogInformation($"Creating budgetRow: {budgetRow}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Creating budgetRow failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task UpdateBudgetCellAsync(BudgetCell budgetCell, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetCellCrud.UpdateAsync(budgetCell, cancellationToken);

            LogInformation($"Updating budgetCell: {budgetCell}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Updating budgetCell failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task UpdateBudgetColumnAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetColumnCrud.UpdateAsync(budgetColumn, cancellationToken);

            LogInformation($"Updating budgetColumn: {budgetColumn}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Updating budgetColumn failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task UpdateBudgetGroupAsync(BudgetGroup budgetGroup, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetGroupCrud.UpdateAsync(budgetGroup, cancellationToken);

            LogInformation($"Updating budgetGroup: {budgetGroup}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Updating budgetGroup failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task UpdateBudgetRowAsync(BudgetRow budgetRow, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetRowCrud.UpdateAsync(budgetRow, cancellationToken);

            LogInformation($"Updating budgetRow: {budgetRow}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Updating budgetRow failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task DeleteBudgetCellAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetCellCrud.DeleteAsync(id, cancellationToken);

            LogInformation($"Deleting budgetCell: {id}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Deleting budgetCell failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task DeleteBudgetColumnAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetColumnCrud.DeleteAsync(id, cancellationToken);

            LogInformation($"Deleting budgetColumn: {id}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Deleting budgetColumn failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task DeleteBudgetGroupAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetGroupCrud.DeleteAsync(id, cancellationToken);

            LogInformation($"Deleting budgetGroup: {id}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Deleting budgetGroup failed: {ex.Message}", DateTime.Now);
        }
    }

    public async Task DeleteBudgetRowAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _budgetRowCrud.DeleteAsync(id, cancellationToken);

            LogInformation($"Deleting budgetRow: {id}", DateTime.Now);
        }
        catch (Exception ex)
        {
            LogError($"Deleting budgetRow failed: {ex.Message}", DateTime.Now);
        }
    }
}
