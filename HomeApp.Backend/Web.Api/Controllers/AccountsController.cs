using Application.Features.Users.Commands.EmailConfirmation;
using Application.Features.Users.Commands.ForgotPassword;
using Application.Features.Users.Commands.Register;
using Application.Features.Users.Commands.ResetPassword;
using Application.Features.Users.Queries.GetAllUser;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController(
    IMediator mediator)
    : ControllerBase
{
    [HttpGet("confirm-email")]
    public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new EmailConfirmationCommand(email, token), cancellationToken);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand forgotPasswordDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await mediator.Send(forgotPasswordDto, cancellationToken);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response);
    }

    [Authorize]
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllUserQuery(), cancellationToken);

        if (response.IsSuccess) return Ok(response.Value);

        return BadRequest(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterAccountCommand registerAccountCommand,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await mediator.Send(registerAccountCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response.IsSuccess);

        return BadRequest(response);
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPasswordDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await mediator.Send(resetPasswordDto, cancellationToken);

        if (response.IsSuccess) return Ok(response.IsSuccess);

        return BadRequest(response);
    }
}
