using Application.Users.Commands.ForgotPassword;
using Application.Users.Commands.Register;
using Application.Users.Queries.GetAllUser;
using Domain.Entities.User;
using HomeApp.Identity.Entities.DataTransferObjects.ResetPassword;
using Microsoft.AspNetCore.Identity;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController(
    IMediator mediator,
    UserManager<User> userManager)
    : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;

    [HttpGet("confirm-email")]
    public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
            return BadRequest("Invalid Email Confirmation Request");

        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);

        if (!confirmResult.Succeeded)
            return BadRequest("Invalid Email Confirmation Request");

        return Ok();
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
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserCommand registerUserCommand,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await mediator.Send(registerUserCommand, cancellationToken);

        if (response.IsSuccess) return Ok(response.IsSuccess);

        return BadRequest(response);
    }

    [Authorize]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user is null || string.IsNullOrWhiteSpace(resetPasswordDto.Token))
            return BadRequest("Invalid Request");

        var resetPassResult =
            await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

        if (!resetPassResult.Succeeded)
        {
            var errors = resetPassResult.Errors.Select(x => x.Description);

            return BadRequest(new { Errors = errors });
        }

        await _userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

        return Ok();
    }
}
