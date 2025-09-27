using Application.Features.Budgets.Commands;
using Application.Features.Budgets.Queries;
using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;
using SharedKernel;
using Web.Api.Requests.Budget;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
// [Authorize(Policy = "ViewBudgetPolicy")]
[Authorize]
[Route("[controller]")]
public class BudgetController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    public async Task<IActionResult> GetBudgetAsync([FromQuery] int year, CancellationToken cancellationToken)
    {
        if (year <= 0)
            year = DateTime.Now.Year;

        var response = await _mediator.Send(new GetBudgetQuery(year));

        if (response.IsSuccess) return Ok(response.Value);

        if (response.Error.Type == ErrorType.NotFound)
            return NoContent();

        return BadRequest(response.Error);
    }

    [HttpPost("")]
    public async Task<IActionResult> PostBudgetAsync([FromQuery] int year,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateBudgetCommand(year));

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response.Error);
    }

    [HttpPost("cell")]
    public async Task<ActionResult<BudgetCell>> PostBudgetCellAsync(
        [FromBody] CreateBudgetCellRequest createBudgetGroupRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateBudgetCellCommand)createBudgetGroupRequest);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response.Error);
    }

    [HttpPost("group")]
    public async Task<IActionResult> PostBudgetGroupAsync([FromBody] CreateBudgetGroupRequest createBudgetGroupRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateBudgetGroupCommand)createBudgetGroupRequest);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response.Error);
    }

    [HttpPost("row")]
    public async Task<IActionResult> PostBudgetRowAsync([FromBody] CreateBudgetRowRequest createBudgetRowRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateBudgetRowCommand)createBudgetRowRequest);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response.Error);
    }
    //
    // [HttpPut("cell")]
    // public async Task<IActionResult> PutBudgetCellAsync([FromBody] BudgetCell budgetCell,
    //     CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.UpdateBudgetCellAsync(budgetCell, cancellationToken);
    //
    //     return Ok();
    // }
    //
    // [HttpPut("column")]
    // public async Task<IActionResult> PutBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
    //     CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.UpdateBudgetColumnAsync(budgetColumn, cancellationToken);
    //
    //     return Ok();
    // }
    //
    // [HttpPut("group")]
    // public async Task<IActionResult> PutBudgetGroupAsync([FromBody] BudgetGroup budgetGroup,
    //     CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.UpdateBudgetGroupAsync(budgetGroup, cancellationToken);
    //
    //     return Ok();
    // }
    //
    // [HttpPut("row")]
    // public async Task<IActionResult> PutBudgetRowAsync([FromBody] BudgetRow budgetRow,
    //     CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.UpdateBudgetRowAsync(budgetRow, cancellationToken);
    //
    //     return Ok();
    // }
    //
    // [HttpDelete("cell/{id}")]
    // public async Task<IActionResult> DeleteBudgetCellAsync(int id, CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.DeleteBudgetCellAsync(id, cancellationToken);
    //
    //     return Ok();
    // }
    //
    // [HttpDelete("column/{id}")]
    // public async Task<IActionResult> DeleteBudgetColumnAsync(int id, CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.DeleteBudgetColumnAsync(id, cancellationToken);
    //
    //     return Ok();
    // }
    //
    // [HttpDelete("group/{id}")]
    // public async Task<IActionResult> DeleteBudgetGroupAsync(int id, CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.DeleteBudgetGroupAsync(id, cancellationToken);
    //
    //     return Ok();
    // }
    //
    // [HttpDelete("row/{id}")]
    // public async Task<IActionResult> DeleteBudgetRowAsync(int id, CancellationToken cancellationToken)
    // {
    //     await _budgetFacade.DeleteBudgetRowAsync(id, cancellationToken);
    //
    //     return Ok();
    // }
}
