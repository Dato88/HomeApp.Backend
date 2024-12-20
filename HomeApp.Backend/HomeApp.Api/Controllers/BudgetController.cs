using HomeApp.DataAccess.Models;
using HomeApp.Library.Facades.Interfaces;
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
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken) =>
        Ok(await _budgetFacade.GetBudgetAsync(cancellationToken));

    [HttpPost("cell")]
    public async Task<ActionResult<BudgetCell>> PostBudgetCellAsync([FromBody] BudgetCell budgetCell,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetCellAsync(budgetCell, cancellationToken);

        return Ok(budgetCell);
    }

    [HttpPost("column")]
    public async Task<IActionResult> PostBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetColumnAsync(budgetColumn, cancellationToken);

        return Ok(budgetColumn);
    }

    [HttpPost("group")]
    public async Task<IActionResult> PostBudgetGroupAsync([FromBody] BudgetGroup budgetGroup,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetGroupAsync(budgetGroup, cancellationToken);

        return Ok(budgetGroup);
    }

    [HttpPost("row")]
    public async Task<IActionResult> PostBudgetRowAsync([FromBody] BudgetRow budgetRow,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetRowAsync(budgetRow, cancellationToken);

        return Ok(budgetRow);
    }

    [HttpPut("cell")]
    public async Task<IActionResult> PutBudgetCellAsync([FromBody] BudgetCell budgetCell,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.UpdateBudgetCellAsync(budgetCell, cancellationToken);

        return Ok();
    }

    [HttpPut("column")]
    public async Task<IActionResult> PutBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.UpdateBudgetColumnAsync(budgetColumn, cancellationToken);

        return Ok();
    }

    [HttpPut("group")]
    public async Task<IActionResult> PutBudgetGroupAsync([FromBody] BudgetGroup budgetGroup,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.UpdateBudgetGroupAsync(budgetGroup, cancellationToken);

        return Ok();
    }

    [HttpPut("row")]
    public async Task<IActionResult> PutBudgetRowAsync([FromBody] BudgetRow budgetRow,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.UpdateBudgetRowAsync(budgetRow, cancellationToken);

        return Ok();
    }

    [HttpDelete("cell/{id}")]
    public async Task<IActionResult> DeleteBudgetCellAsync(int id, CancellationToken cancellationToken)
    {
        await _budgetFacade.DeleteBudgetCellAsync(id, cancellationToken);

        return Ok();
    }

    [HttpDelete("column/{id}")]
    public async Task<IActionResult> DeleteBudgetColumnAsync(int id, CancellationToken cancellationToken)
    {
        await _budgetFacade.DeleteBudgetColumnAsync(id, cancellationToken);

        return Ok();
    }

    [HttpDelete("group/{id}")]
    public async Task<IActionResult> DeleteBudgetGroupAsync(int id, CancellationToken cancellationToken)
    {
        await _budgetFacade.DeleteBudgetGroupAsync(id, cancellationToken);

        return Ok();
    }

    [HttpDelete("row/{id}")]
    public async Task<IActionResult> DeleteBudgetRowAsync(int id, CancellationToken cancellationToken)
    {
        await _budgetFacade.DeleteBudgetRowAsync(id, cancellationToken);

        return Ok();
    }
}
