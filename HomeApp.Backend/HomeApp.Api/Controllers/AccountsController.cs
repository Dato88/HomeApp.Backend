using System.IdentityModel.Tokens.Jwt;
using HomeApp.Identity.Handler;
using HomeApp.Identity.Models;
using HomeApp.Identity.Models.Authentication;
using HomeApp.Identity.Models.ResetPassword;
using HomeApp.Library.Email;
using HomeApp.Library.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace HomeApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AccountsController(UserManager<User> userManager, JwtHandler jwtHandler, IEmailSender emailSender)
    : ControllerBase
{
    [HttpPost(Name = "Login")]
    public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthenticationDto)
    {
        var user = await userManager.FindByEmailAsync(userForAuthenticationDto.Email);

        if (user is null)
            BadRequest("Invalid Request");

        if (!await userManager.IsEmailConfirmedAsync(user))
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Email is not confirmed" });

        if (!await userManager.CheckPasswordAsync(user, userForAuthenticationDto.Password))
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

        var signingCredentials = jwtHandler.GetSigningCredentials();
        var claims = await jwtHandler.GetClaims(user);
        var tokenOptions = jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
    }

    [HttpPost(Name = "ForgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);

        if (user is null)
            return BadRequest("Invalid Request");

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var param = new Dictionary<string, string?>
        {
            { "token", token },
            { "email", forgotPasswordDto.Email }
        };

        var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param);

        var message = new Message(new string[] { user.Email }, "Reset password token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        return Ok();
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user is null)
            return BadRequest("Invalid Request");

        var resetPassResult =
            await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

        if (!resetPassResult.Succeeded)
        {
            var errors = resetPassResult.Errors.Select(x => x.Description);

            return BadRequest(new { Errors = errors });
        }

        return Ok();
    }
}