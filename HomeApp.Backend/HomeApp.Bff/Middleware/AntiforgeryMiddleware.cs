using Microsoft.AspNetCore.Antiforgery;

namespace HomeApp.Bff.Middleware;

public sealed class AntiforgeryMiddleware(RequestDelegate next, IAntiforgery antiforgery)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (RequiresValidation(context.Request))
        {
            try
            {
                await antiforgery.ValidateRequestAsync(context);
            }
            catch (AntiforgeryValidationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
        }

        await next(context);
    }

    private static bool RequiresValidation(HttpRequest request)
    {
        if (!request.Path.StartsWithSegments("/api"))
            return false;

        return HttpMethods.IsPost(request.Method)
            || HttpMethods.IsPut(request.Method)
            || HttpMethods.IsPatch(request.Method)
            || HttpMethods.IsDelete(request.Method);
    }
}
