using Application.Abstractions.Authentication;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Authentication;

internal sealed class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    private int? _workerPersonId;

    public int PersonId
    {
        get
        {
            if (_workerPersonId.HasValue)
                return _workerPersonId.Value;

            if (httpContextAccessor.HttpContext?.Items.TryGetValue(
                    DependencyInjection.PersonIdItemKey,
                    out var value) == true
                && value is int personId)
                return personId;

            throw new UnauthorizedAccessException("No valid PersonId in execution context.");
        }
    }

    public Guid KeycloakUserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User
                       ?? throw new UnauthorizedAccessException("No authenticated user in execution context.");

            return user.GetKeycloakUserId()
                   ?? throw new UnauthorizedAccessException("JWT is missing sub claim.");
        }
    }

    public UserEmail Email
    {
        get
        {
            var user = httpContextAccessor.HttpContext?.User
                       ?? throw new UnauthorizedAccessException("No authenticated user in execution context.");

            return user.GetUserEmail()
                   ?? throw new UnauthorizedAccessException("JWT is missing email claim.");
        }
    }

    public void SetWorkerPersonId(int personId) => _workerPersonId = personId;
}
