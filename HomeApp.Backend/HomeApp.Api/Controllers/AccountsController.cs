using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Entities.DataTransferObjects.Register;
using HomeApp.Identity.Entities.DataTransferObjects.ResetPassword;
using HomeApp.Identity.Entities.Models;
using HomeApp.Library.Email;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace HomeApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController(
    IUserCrud userCrud,
    IEmailSender emailSender,
    IPersonFacade personFacade,
    UserManager<User> userManager)
    : ControllerBase
{
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IPersonFacade _personFacade = personFacade;
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

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var allUsers = await _userCrud.GetAllUsersAsync(cancellationToken);
        return Ok(allUsers);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userCrud.RegisterAsync(registerUserDto, cancellationToken);

        if (!result.Item1.Succeeded)
        {
            var errors = result.Item1.Errors.Select(e => e.Description);

            return BadRequest(new RegistrationResponseDto { Errors = errors });
        }

        await _personFacade.CreatePersonAsync(result.Item2, cancellationToken);

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(registerUserDto);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", registerUserDto.Email } };
        var callback = registerUserDto.ClientUri is null
            ? string.Empty
            : QueryHelpers.AddQueryString(registerUserDto.ClientUri, param);
        var message = new Message(new[] { registerUserDto.Email }, "Email Confirmation token", callback);

        await _emailSender.SendEmailAsync(message, cancellationToken);

        return StatusCode(201);
    }

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
