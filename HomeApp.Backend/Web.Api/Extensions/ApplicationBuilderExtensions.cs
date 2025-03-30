using Scalar.AspNetCore;

namespace Web.Api.Extensions;

internal static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseScalarApiWithUi(this WebApplication app)
    {
        app.MapOpenApi();
        app.MapScalarApiReference();

        return app;
    }

    public static IApplicationBuilder UseAuthenticationExtension(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseCors("CorsPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}
