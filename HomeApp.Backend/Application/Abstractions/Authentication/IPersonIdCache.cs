namespace Application.Abstractions.Authentication;

public interface IPersonIdCache
{
    bool TryGetPersonId(string keycloakUserId, out int personId);

    void SetPersonId(string keycloakUserId, int personId);

    void Remove(string keycloakUserId);
}
