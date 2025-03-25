using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(UserManager<User> userManager, IConfiguration configuration) : ITokenProvider
{
    public string Create(User user)
    {
        var secretKey = configuration["JWTSettings:securityKey"]!;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(GetClaims(user).Result),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("JWTSettings:expiryInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["JWTSettings:validIssuer"],
            Audience = configuration["JWTSettings:validAudience"]
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim> { new(ClaimTypes.Name, user.Email) };

        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }
}
