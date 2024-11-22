using System.IdentityModel.Tokens.Jwt;
using HomeApp.Identity.Handler;
using HomeApp.Identity.Models;
using HomeApp.Identity.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace HomeApp.Api.Controllers;

public class AccountsController(UserManager<User> userManager, JwtHandler jwtHandler) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly JwtHandler _jwtHandler = jwtHandler;

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthenticationDto)
    {
        var user = await _userManager.FindByEmailAsync(userForAuthenticationDto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthenticationDto.Password))
            return Unauthorized(new AuthResponseDto { ErrorMessage = "Invalid Authentication" });

        var signingCredentials = _jwtHandler.GetSigningCredentials();
        var claims = _jwtHandler.GetClaims(user);
        var tokenOptions = _jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return Ok(new AuthResponseDto { IsAuthSuccessful = true, Token = token });
    }
}