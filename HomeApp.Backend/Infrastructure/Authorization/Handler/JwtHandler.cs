using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authorization.Handler;

public class JwtHandler(UserManager<User> userManager, IConfiguration configuration)
{
    private readonly IConfigurationSection _jwtSettings = configuration.GetSection("JwtSettings");
    private readonly UserManager<User> _userManager = userManager;

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim> { new(ClaimTypes.Name, user.Email) };

        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

        return claims;
    }

    public async Task<string> GenerateToken(User user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            _jwtSettings["validIssuer"],
            _jwtSettings["validAudience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }
}
