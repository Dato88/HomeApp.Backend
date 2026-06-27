using Application;
using FluentValidation;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Database;
using Infrastructure.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Web.Api;
using Web.Api.Extensions;
using AssemblyReference = Web.Api.AssemblyReference;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddPresentation();

builder.AddInfrastructureTelemetry();

builder.Services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);

var app = builder.Build();

app.UseHealthChecksExtension();

var applyMigrations = app.Configuration.GetValue<bool>("Database:ApplyMigrations");

if (applyMigrations)
    await app.MigrateDatabaseAsync<HomeAppContext>("HomeAppContext");

if (app.Environment.IsDevelopment())
    app.UseScalarApiWithUi();

app.UseAuthenticationExtension();

if (Infrastructure.DependencyInjection.IsOAuthConfigured(app.Configuration))
    app.UseMiddleware<PersonProvisioningMiddleware>();

app.MapControllers().RequireCors("CorsPolicy");

await app.RunAsync();

public partial class Program;
