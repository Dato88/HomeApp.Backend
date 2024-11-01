using System.Security.Claims;

namespace HomeApp.Identity.Utilities;

public static class ClaimStore
{
    public static List<Claim> AllClaims = new()
    {
        new("View DashBoard", "View DashBoard"),
        new("View Budget", "View Budget"),
    };
}