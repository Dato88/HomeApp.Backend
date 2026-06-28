using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Middleware;

public sealed class PersonProvisioningMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IPersonProvisioningService personProvisioningService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var logger = context.RequestServices.GetRequiredService<IAppLogger<PersonProvisioningMiddleware>>();

            if (context.User.GetKeycloakUserId() is null)
            {
                logger.LogWarning("Authenticated request is missing sub claim.");
                throw new UnauthorizedAccessException("JWT is missing sub claim.");
            }

            var personId = await personProvisioningService.EnsurePersonAsync(context.User, context.RequestAborted);
            context.Items[DependencyInjection.PersonIdItemKey] = personId;
        }

        await next(context);
    }
}
