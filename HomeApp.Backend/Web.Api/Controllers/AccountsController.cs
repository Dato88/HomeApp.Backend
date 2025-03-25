using Application.Cruds.Interfaces;
using Application.Email;
using Application.Models.Email;
using Application.Users.Commands.Register;
using Application.Users.Queries.GetAllUser;
using Domain.Entities.User;
using HomeApp.Identity.Entities.DataTransferObjects.ResetPassword;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController(
    IUserCrud userCrud,
    IEmailSender emailSender,
    IMediator mediator,
    UserManager<User> userManager)
    : ControllerBase
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUserCrud _userCrud = userCrud;
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
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

        if (user?.Email is null)
            return BadRequest("Invalid Request");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", forgotPasswordDto.Email } };

        var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

        var message = new Message(new[] { user.Email }, "Reset password token", callback);

        await _emailSender.SendEmailAsync(message, cancellationToken);

        return Ok();
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
