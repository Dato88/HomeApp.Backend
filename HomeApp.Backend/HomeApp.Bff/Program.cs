using System.Threading.RateLimiting;
using HomeApp.Bff.Configuration;
using HomeApp.Bff.Middleware;
using HomeApp.Bff.Proxy;
using HomeApp.Bff.Services;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<BffOAuthOptions>(builder.Configuration.GetSection(BffOAuthOptions.SectionName));

builder.Services.AddHttpClient("keycloak");
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<PkceService>();
builder.Services.AddScoped<OAuthService>();
builder.Services.AddScoped<TokenSessionService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

var angularOrigin = builder.Configuration["OAuth:AngularOrigin"] ?? "http://localhost:4200";

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins(angularOrigin)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("auth", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1)
            }));
});

var reverseProxyBuilder = builder.Services
    .AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms<TokenTransformProvider>();

if (builder.Environment.IsDevelopment())
{
    reverseProxyBuilder.ConfigureHttpClient((_, handler) =>
    {
        handler.SslOptions.RemoteCertificateValidationCallback =
            static (_, _, _, _) => true;
    });
}

builder.Services.AddControllers();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseRateLimiter();
app.UseCors("Angular");
app.UseSession();
app.UseMiddleware<AntiforgeryMiddleware>();
app.MapControllers();
app.MapReverseProxy();

app.Run();
