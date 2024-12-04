using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Entities.DataTransferObjects.Register;
using HomeApp.Identity.Entities.DataTransferObjects.ResetPassword;
using HomeApp.Identity.Entities.Models;
using HomeApp.Library.Email;
using HomeApp.Library.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Http;

namespace HomeApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountsController(
    IUserCrud userCrud,
    IEmailSender emailSender,
    UserManager<User> userManager)
    : ControllerBase
{
    [HttpGet("EmailConfirmation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return BadRequest("Invalid Email Confirmation Request");

        var confirmResult = await userManager.ConfirmEmailAsync(user, token);

        if (!confirmResult.Succeeded)
            return BadRequest("Invalid Email Confirmation Request");

        return Ok();
    }

    [HttpPost(Name = "ForgotPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary),
        StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);

        if (user?.Email is null)
            return BadRequest("Invalid Request");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", forgotPasswordDto.Email } };

        var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

        var message = new Message(new string[] { user.Email }, "Reset password token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        return Ok();
    }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var allUsers = await userCrud.GetAllUsersAsync(cancellationToken);

        return allUsers;
    }

    [HttpPost(Name = "Register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary),
        StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await userCrud.RegisterAsync(registerUserDto, cancellationToken);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            return BadRequest(new RegistrationResponseDto { Errors = errors });
        }

        var token = await userManager.GenerateEmailConfirmationTokenAsync(registerUserDto);
        var param = new Dictionary<string, string?> { { "token", token }, { "email", registerUserDto.Email } };
        var callback = registerUserDto.ClientUri is null
            ? string.Empty
            : QueryHelpers.AddQueryString(registerUserDto.ClientUri, param);
        var message = new Message(new string[] { registerUserDto.Email }, "Email Confirmation token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        return StatusCode(201);
    }

    [HttpPost("ResetPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary),
        StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user is null || resetPasswordDto.Token is null)
            return BadRequest("Invalid Request");

        var resetPassResult =
            await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

        if (!resetPassResult.Succeeded)
        {
            var errors = resetPassResult.Errors.Select(x => x.Description);

            return BadRequest(new { Errors = errors });
        }

        await userManager.SetLockoutEndDateAsync(user, new DateTime(2000, 1, 1));

        return Ok();
    }
}
