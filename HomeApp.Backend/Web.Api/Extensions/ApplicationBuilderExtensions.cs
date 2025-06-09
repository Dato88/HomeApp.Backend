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

        app.UseCors("CorsPolicy");

        app.UseAuthentication();

        app.UseAuthorization();

        return app;
    }
}
