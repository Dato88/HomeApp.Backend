using HomeApp.Bff.Services;
using Microsoft.Extensions.DependencyInjection;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace HomeApp.Bff.Proxy;

public sealed class TokenTransformProvider : ITransformProvider
{
    public void ValidateRoute(TransformRouteValidationContext context)
    {
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
    }

    public void Apply(TransformBuilderContext context)
    {
        context.AddRequestTransform(async transformContext =>
        {
            var httpContext = transformContext.HttpContext;
            var tokenSession = httpContext.RequestServices.GetRequiredService<TokenSessionService>();

            await tokenSession.EnsureValidAccessTokenAsync(httpContext.RequestAborted);

            var accessToken = tokenSession.GetAccessToken();
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                transformContext.ProxyRequest.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
        });
    }
}
