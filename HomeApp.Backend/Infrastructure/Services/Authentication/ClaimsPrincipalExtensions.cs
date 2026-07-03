using System.Security.Claims;
using Domain.ValueObjects;

namespace Infrastructure.Services.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid? GetKeycloakUserId(this ClaimsPrincipal? principal)
    {
        var sub = principal?.FindFirstValue("sub")
                  ?? principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(sub, out var userId) ? userId : null;
    }

    public static UserEmail? GetUserEmail(this ClaimsPrincipal? principal)
    {
        var email = principal?.FindFirstValue("email")
                    ?? principal?.FindFirstValue(ClaimTypes.Email);

        return string.IsNullOrWhiteSpace(email) ? null : new UserEmail(email);
    }

    public static string? GetFirstName(this ClaimsPrincipal? principal) =>
        principal?.FindFirstValue("given_name");

    public static string? GetLastName(this ClaimsPrincipal? principal) =>
        principal?.FindFirstValue("family_name");

    public static string? GetPreferredUsername(this ClaimsPrincipal? principal) =>
        principal?.FindFirstValue("preferred_username");

    public static IReadOnlyList<string> GetRealmRoles(this ClaimsPrincipal? principal)
    {
        if (principal is null)
            return [];

        return principal
            .FindAll("roles")
            .Select(claim => claim.Value)
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .ToList();
    }
}
