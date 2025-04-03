using System.Security.Claims;

namespace Infrastructure.Services.Authentication;

internal static class ClaimsPrincipalExtensions
{
    public static int GetPersonId(this ClaimsPrincipal? principal)
    {
        var personId = principal?.FindFirstValue("personId");

        return int.TryParse(personId, out var parsedPersonId)
            ? parsedPersonId
            : throw new ApplicationException("Person id is unavailable");
    }

    public static string GetUserEmail(this ClaimsPrincipal? principal)
    {
        var userEmail = principal?.FindFirstValue(ClaimTypes.Email);

        return !string.IsNullOrEmpty(userEmail)
            ? userEmail
            : throw new ApplicationException("User email is unavailable");
    }

    public static Guid GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userId, out var parsedUserId)
            ? parsedUserId
            : throw new ApplicationException("User id is unavailable");
    }
}
