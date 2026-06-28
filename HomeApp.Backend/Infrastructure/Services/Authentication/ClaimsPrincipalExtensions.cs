using System.Security.Claims;
using System.Text.Json;
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
        var rolesClaim = principal?.FindFirst("realm_access")?.Value;

        if (string.IsNullOrWhiteSpace(rolesClaim))
            return [];

        try
        {
            using var document = JsonDocument.Parse(rolesClaim);

            if (!document.RootElement.TryGetProperty("roles", out var rolesElement)
                || rolesElement.ValueKind != JsonValueKind.Array)
                return [];

            return rolesElement.EnumerateArray()
                .Select(role => role.GetString())
                .Where(role => !string.IsNullOrWhiteSpace(role))
                .Select(role => role!)
                .ToList();
        }
        catch (JsonException)
        {
            return [];
        }
    }
}
