using Domain.ValueObjects;

namespace Application.Abstractions.Authentication;

public interface IExecutionContextAccessor
{
    int PersonId { get; }
    Guid KeycloakUserId { get; }
    UserEmail Email { get; }
    void SetWorkerPersonId(int personId);
}
