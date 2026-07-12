using Application.Features.Finance.CategoryGroups.Commands;
using Application.Features.Finance.CategoryGroups.Queries;
using Application.Features.Finance.Dtos;
using SharedKernel;
using Web.Api.Requests.Finance;

namespace Web.Api.Controllers;

[ApiController]
[Authorize(Roles = "ViewFinance")]
[Route("[controller]")]
public class CategoryGroupController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<CategoryGroupDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetCategoryGroupsAsync([FromQuery] int householdId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCategoryGroupsQuery(householdId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostCategoryGroupAsync(
        [FromBody] CreateCategoryGroupRequest createCategoryGroupRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateCategoryGroupCommand)createCategoryGroupRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UpdateCategoryGroupAsync(
        [FromBody] UpdateCategoryGroupRequest updateCategoryGroupRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((UpdateCategoryGroupCommand)updateCategoryGroupRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteCategoryGroupAsync([FromQuery] int categoryGroupId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteCategoryGroupCommand(categoryGroupId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
