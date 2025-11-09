using Application.Features.People.Dtos;
using Application.Features.People.Queries;
using SharedKernel;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PersonController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("person")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<PersonResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetPerson(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserPersonQuery(), cancellationToken);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
