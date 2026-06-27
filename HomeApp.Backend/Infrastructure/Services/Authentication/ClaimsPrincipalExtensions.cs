using System.Security.Claims;
using System.Text.Json;
using Domain.ValueObjects;

namespace Infrastructure.Services.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static Guid? GetUserId(this ClaimsPrincipal? principal)
    {
        var sub = principal?.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? principal?.FindFirstValue("sub");

        return Guid.TryParse(sub, out var userId) ? userId : null;
    }

    public static UserEmail? GetUserEmail(this ClaimsPrincipal? principal)
    {
        var email = principal?.FindFirstValue(ClaimTypes.Email)
                    ?? principal?.FindFirstValue("email");

        return string.IsNullOrWhiteSpace(email) ? null : new UserEmail(email);
    }

    public static int? GetPersonId(this ClaimsPrincipal? principal)
    {
        var personIdClaim = principal?.FindFirstValue("personId");

        return int.TryParse(personIdClaim, out var personId) ? personId : null;
    }

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
