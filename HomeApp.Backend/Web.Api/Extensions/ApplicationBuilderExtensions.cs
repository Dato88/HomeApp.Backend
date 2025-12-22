using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

namespace Web.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseScalarApiWithUi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        return app;
    }

    public static IApplicationBuilder UseAuthenticationExtension(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();

        app.UseExceptionHandler();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }

    public static IApplicationBuilder UseHealthChecksExtension(this WebApplication app)
    {
        app.UseRequestTimeouts();
        app.UseOutputCache();

        app.MapHealthChecks("health",
                new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse })
            .WithRequestTimeout("HealthChecks")
            .CacheOutput("HealthChecks");

        return app;
    }
}
