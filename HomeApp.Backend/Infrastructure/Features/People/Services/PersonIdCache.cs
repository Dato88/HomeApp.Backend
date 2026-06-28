using Application.Abstractions.Authentication;
using Infrastructure.Services.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Features.People.Services;

internal sealed class PersonIdCache(IMemoryCache memoryCache) : IPersonIdCache
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(8);

    public bool TryGetPersonId(string keycloakUserId, out int personId)
    {
        return memoryCache.TryGetValue(PersonIdCacheKeys.ForUserId(keycloakUserId), out personId);
    }

    public void SetPersonId(string keycloakUserId, int personId)
    {
        memoryCache.Set(
            PersonIdCacheKeys.ForUserId(keycloakUserId),
            personId,
            CacheDuration);
    }

    public void Remove(string keycloakUserId)
    {
        memoryCache.Remove(PersonIdCacheKeys.ForUserId(keycloakUserId));
    }
}
