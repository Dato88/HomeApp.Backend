namespace Infrastructure.Services.Authentication;

internal static class PersonIdCacheKeys
{
    public static string ForUserId(string userId) => $"person-id:{userId}";
}
