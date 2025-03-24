using Application.DTOs.Authentication;
using Application.Users.Commands.Login;
using Domain.Entities.User;
using Infrastructure.Authorization.Handler;
using Microsoft.AspNetCore.Identity;

namespace Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController(
    JwtHandler jwtHandler,
    UserManager<User> userManager,
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
