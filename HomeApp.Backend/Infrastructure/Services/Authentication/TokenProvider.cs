using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Domain.Entities.User;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Authentication;

internal sealed class TokenProvider(
    HomeAppContext homeAppContext,
    UserManager<User> userManager,
    IConfiguration configuration) : ITokenProvider
{
    private readonly IConfigurationSection _jwtSettings = configuration.GetSection("JwtSettings");

    public async Task<string> Create(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSecurityToken = new JwtSecurityToken(
            _jwtSettings["validIssuer"],
            _jwtSettings["validAudience"],
            claims,
            expires: DateTime.Now.AddMinutes(_jwtSettings.GetValue<int>("expiryInMinutes")),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var person = await homeAppContext.People.FirstOrDefaultAsync(x => x.Email == user.Email);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new("personId", person.Id.ToString())
        };

        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}
