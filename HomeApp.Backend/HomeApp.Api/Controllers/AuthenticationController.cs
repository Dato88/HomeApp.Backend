﻿using HomeApp.Identity.Entities.DataTransferObjects.Authentication;
using HomeApp.Identity.Entities.Models;
using HomeApp.Identity.Handler;
using HomeApp.Library.Email;
using HomeApp.Library.Models.Email;
using Microsoft.AspNetCore.Identity;

namespace HomeApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(
    JwtHandler jwtHandler,
    UserManager<User> userManager,
    IEmailSender emailSender)
    : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthenticationDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(userForAuthenticationDto.Email);

        if (user is null)
            BadRequest("Invalid Request");

        if (!await userManager.IsEmailConfirmedAsync(user))
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Email is not confirmed" });

        if (!await userManager.CheckPasswordAsync(user, userForAuthenticationDto.Password))
        {
            await userManager.AccessFailedAsync(user);

            if (!await userManager.IsLockedOutAsync(user))
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

            var content =
                $"Your account is locked out. To reset the password click this link: {userForAuthenticationDto.ClientUri}";
            var message = new Message(new[] { userForAuthenticationDto.Email },
                "Locked out account information", content);

            await emailSender.SendEmailAsync(message, cancellationToken);

            return Unauthorized(new AuthResponseDto { ErrorMessage = "The account is locked out" });
        }

        if (await userManager.GetTwoFactorEnabledAsync(user))
            return await GenerateOtpFor2StepVerification(user, cancellationToken);

        var token = await jwtHandler.GenerateToken(user);

        await userManager.ResetAccessFailedCountAsync(user);

        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
    }

    private async Task<IActionResult> GenerateOtpFor2StepVerification(User user, CancellationToken cancellationToken)
    {
        var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
        if (!providers.Contains("Email"))
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid 2-Step Verification Provider." });

        var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
        var message = new Message(new[] { user.Email }, "Authentication token", token);

        await emailSender.SendEmailAsync(message, cancellationToken);

        return Ok(new AuthResponseDto { Is2StepVerificationRequired = true, Provider = "Email" });
    }

    [HttpPost("2fa-verify")]
    public async Task<IActionResult> TwoStepVerification([FromBody] TwoFactorDto twoFactorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.FindByEmailAsync(twoFactorDto.Email);

        if (user is null)
            return BadRequest("Invalid Request");

        var validVerification =
            await userManager.VerifyTwoFactorTokenAsync(user, twoFactorDto.Provider, twoFactorDto.Token);

        if (!validVerification)
            return BadRequest("Invalid Token Verification");

        var token = await jwtHandler.GenerateToken(user);

        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
    }
}
