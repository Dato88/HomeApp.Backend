using HomeApp.DataAccess.Models;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers;

[ApiController]
[Authorize]
// [Authorize(Policy = "ViewBudgetPolicy")]
[Route("[controller]")]
public class BudgetController(IBudgetFacade budgetFacade) : ControllerBase
{
    private readonly IBudgetFacade _budgetFacade = budgetFacade;

    [HttpGet("budget")]
    public async Task<Budget?> GetAllAsync(CancellationToken cancellationToken) =>
        await _budgetFacade.GetBudgetAsync(cancellationToken);

    [HttpPost("cell")]
    public async Task<BudgetCell> PostBudgetCellAsync([FromBody] BudgetCell budgetCell,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetCellAsync(budgetCell, cancellationToken);
        return budgetCell;
    }

    [HttpPost("column")]
    public async Task<BudgetColumn> PostBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetColumnAsync(budgetColumn, cancellationToken);
        return budgetColumn;
    }

    [HttpPost("group")]
    public async Task<BudgetGroup> PostBudgetGroupAsync([FromBody] BudgetGroup budgetGroup,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetGroupAsync(budgetGroup, cancellationToken);
        return budgetGroup;
    }

    [HttpPost("row")]
    public async Task<BudgetRow> PostBudgetRowAsync([FromBody] BudgetRow budgetRow,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetRowAsync(budgetRow, cancellationToken);
        return budgetRow;
    }

    [HttpPut("cell")]
    public async Task PutBudgetCellAsync([FromBody] BudgetCell budgetCell, CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetCellAsync(budgetCell, cancellationToken);

    [HttpPut("column")]
    public async Task PutBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
        CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetColumnAsync(budgetColumn, cancellationToken);

    [HttpPut("group")]
    public async Task PutBudgetGroupAsync([FromBody] BudgetGroup budgetGroup, CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetGroupAsync(budgetGroup, cancellationToken);

    [HttpPut("row")]
    public async Task PutBudgetRowAsync([FromBody] BudgetRow budgetRow, CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetRowAsync(budgetRow, cancellationToken);

    [HttpDelete("cell/{id}")]
    public async Task DeleteBudgetCellAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetCellAsync(id, cancellationToken);

    [HttpDelete("column/{id}")]
    public async Task DeleteBudgetColumnAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetColumnAsync(id, cancellationToken);

    [HttpDelete("group/{id}")]
    public async Task DeleteBudgetGroupAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetGroupAsync(id, cancellationToken);

    [HttpDelete("row/{id}")]
    public async Task DeleteBudgetRowAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetRowAsync(id, cancellationToken);
}
