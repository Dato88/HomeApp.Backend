using Application.Features.Users.Commands.Login;
using Application.Features.Users.Commands.TwoStepVerification;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(
    IMediator mediator)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await mediator.Send(loginUserCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response);
    }

    [HttpPost("2fa-verify")]
    public async Task<IActionResult> TwoStepVerification(
        [FromBody] TwoStepVerificationCommand twoStepVerificationCommand, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await mediator.Send(twoStepVerificationCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response);
    }
}
