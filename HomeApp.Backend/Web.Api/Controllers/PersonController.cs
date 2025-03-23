using Application.People.Queries;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PersonController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("person")]
    public async Task<IActionResult> GetPerson(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetUserPersonQuery(), cancellationToken);

        if (response.Success) return Ok(response);

        return BadRequest(response);
    }
}
