using System.Security.Claims;

namespace Application.Abstractions.Authentication;

public interface IPersonProvisioningService
{
    Task<int> EnsurePersonAsync(ClaimsPrincipal user, CancellationToken cancellationToken);
}
