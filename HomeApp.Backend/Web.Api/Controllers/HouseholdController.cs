using Application.Features.Households.Commands;
using Application.Features.Households.Dtos;
using Application.Features.Households.Queries;
using SharedKernel;
using Web.Api.Requests.Household;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class HouseholdController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<HouseholdResponse>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetHouseholdsAsync(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetHouseholdsQuery());

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostHouseholdAsync([FromBody] CreateHouseholdRequest createHouseholdRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateHouseholdCommand)createHouseholdRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UpdateHouseholdAsync([FromQuery] int householdId, [FromQuery] string name,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UpdateHouseholdCommand(householdId, name));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteHouseholdAsync([FromQuery] int householdId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteHouseholdCommand(householdId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("member")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostHouseholdMemberAsync(
        [FromBody] AddHouseholdMemberRequest addHouseholdMemberRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((AddHouseholdMemberCommand)addHouseholdMemberRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("member")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteHouseholdMemberAsync([FromQuery] int householdId,
        [FromQuery] int personId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new RemoveHouseholdMemberCommand(householdId, personId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
