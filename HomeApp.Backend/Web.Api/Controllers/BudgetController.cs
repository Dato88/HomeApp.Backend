using Application.Features.Budgets.Commands.Create;
using Application.Features.Budgets.Commands.Delete;
using Application.Features.Budgets.Commands.Update;
using Application.Features.Budgets.DTOs;
using Application.Features.Budgets.Queries;
using Domain.Entities.Budgets;
using SharedKernel;
using Web.Api.Requests.Budget;

namespace Web.Api.Controllers;

[ApiController]
[Authorize(Policy = "ViewBudgetPolicy")]
[Route("[controller]")]
public class BudgetController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<BudgetResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetBudgetAsync([FromQuery] int year, CancellationToken cancellationToken)
    {
        if (year <= 0)
            year = DateTime.Now.Year;

        var response = await _mediator.Send(new GetBudgetQuery(year));

        if (response.IsSuccess) return Ok(response);

        if (response.Error.Type == ErrorType.NotFound)
            return NoContent();

        return BadRequest(response.Error);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostBudgetAsync([FromQuery] int year,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateBudgetCommand(year));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("cell")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<ActionResult<BudgetCell>> PostBudgetCellAsync(
        [FromBody] CreateBudgetCellRequest createBudgetGroupRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateBudgetCellCommand)createBudgetGroupRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostBudgetGroupAsync([FromBody] CreateBudgetGroupRequest createBudgetGroupRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateBudgetGroupCommand)createBudgetGroupRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("row")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostBudgetRowAsync([FromBody] CreateBudgetRowRequest createBudgetRowRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateBudgetRowCommand)createBudgetRowRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteBudgetAsync([FromQuery] int budgetId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteBudgetCommand(budgetId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("group")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteBudgetGroupAsync([FromQuery] int budgetRowId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteBudgetGroupCommand(budgetRowId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("row")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteBudgetRowAsync([FromQuery] int budgetRowId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteBudgetRowCommand(budgetRowId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("cell")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteBudgetCellAsync([FromQuery] int budgetCellId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteBudgetCellCommand(budgetCellId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UpdateBudgetAsync([FromQuery] int budgetId, [FromQuery] int year,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UpdateBudgetCommand(budgetId, year));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
