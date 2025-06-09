using System.Text.Json;

namespace Web.Api.Middleware;

public class ValidationExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        await _next(context);

        if (context.Response.StatusCode == 400 &&
            context.Response.ContentType?.Contains("application/problem+json") == true)
        {
            newBodyStream.Seek(0, SeekOrigin.Begin);
            var problemJson = await new StreamReader(newBodyStream).ReadToEndAsync();

            var problem = JsonSerializer.Deserialize<ValidationProblemDetails>(problemJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var baseResponse = new
            {
                isSuccess = false,
                value = (object)null,
                message = problem?.Title,
                errors = problem?.Errors.SelectMany(e => e.Value.Select(msg => new { code = e.Key, message = msg }))
            };

            context.Response.ContentType = "application/json";
            context.Response.ContentLength = null;
            newBodyStream.SetLength(0);
            newBodyStream.Seek(0, SeekOrigin.Begin);
            await JsonSerializer.SerializeAsync(context.Response.Body, baseResponse);
        }

        newBodyStream.Seek(0, SeekOrigin.Begin);
        await newBodyStream.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;
    }
}
