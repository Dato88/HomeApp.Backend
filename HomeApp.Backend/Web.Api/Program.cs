using System.Text;
using Application;
using FluentValidation;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Web.Api;
using Web.Api.Extensions;
using Web.Api.Middleware;
using AssemblyReference = Web.Api.AssemblyReference;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSettings.GetSection("securityKey").Value))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseScalarApiWithUi();

app.UseMiddleware<ValidationExceptionHandlingMiddleware>();

app.MapHealthChecks("health", new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

app.UseAuthenticationExtension();

app.MapControllers().RequireCors("CorsPolicy");

await app.RunAsync();

public partial class Program
{
}
