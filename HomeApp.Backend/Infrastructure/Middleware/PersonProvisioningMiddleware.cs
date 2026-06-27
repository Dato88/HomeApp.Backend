using Application.Abstractions.Authentication;
using Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Middleware;

public sealed class PersonProvisioningMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext context,
        IPersonProvisioningService personProvisioningService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var personId = await personProvisioningService.EnsurePersonAsync(context.User, context.RequestAborted);
            context.Items[DependencyInjection.PersonIdItemKey] = personId;
        }

        await next(context);
    }
}
