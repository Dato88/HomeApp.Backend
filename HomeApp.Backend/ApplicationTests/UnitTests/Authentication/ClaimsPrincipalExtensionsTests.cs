using System.Security.Claims;
using Infrastructure.Services.Authentication;

namespace ApplicationTests.UnitTests.Authentication;

public class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetRealmRoles_ReturnsRolesFromFlatClaims()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim("roles", "ViewTodo"),
            new Claim("roles", "ViewBudget"),
        ],
        authenticationType: "Bearer"));

        var roles = principal.GetRealmRoles();

        Assert.Equal(["ViewTodo", "ViewBudget"], roles);
    }

    [Fact]
    public void GetRealmRoles_ReturnsEmpty_WhenPrincipalIsNull()
    {
        Assert.Empty(((ClaimsPrincipal?)null).GetRealmRoles());
    }

    [Fact]
    public void GetRealmRoles_ReturnsEmpty_WhenNoRoleClaimsExist()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(authenticationType: "Bearer"));

        Assert.Empty(principal.GetRealmRoles());
    }
}
