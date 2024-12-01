using System.Security.Claims;

namespace HomeApp.Identity.Utilities;

public static class ClaimStore
{
    public static List<Claim> AllClaims = new()
    {
        new Claim("View DashBoard", "View DashBoard"),
        new Claim("View Budget", "View Budget"),
    };
}