using System.Security.Claims;
using Application.Abstractions.Authentication;
using Application.Abstractions.BudgetModule;
using Application.Abstractions.FinanceModule;
using Application.Abstractions.HouseholdModule;
using Application.Abstractions.Logging;
using Application.Features.People.Commands;
using Application.Features.People.Queries;
using Application.Features.People.Validations;
using Application.Features.Todos.Commands;
using Application.Features.Todos.Queries;
using Infrastructure.Configurations;
using Infrastructure.Database;
using Infrastructure.Features.Budgets.Commands;
using Infrastructure.Features.Budgets.Queries;
using Infrastructure.Features.Finance.Commands;
using Infrastructure.Features.Finance.Queries;
using Infrastructure.Features.Households.Commands;
using Infrastructure.Features.Households.Queries;
using Infrastructure.Features.People.Commands;
using Infrastructure.Features.People.Queries;
using Infrastructure.Features.People.Services;
using Infrastructure.Features.People.Validations;
using Infrastructure.Features.Todos.Commands;
using Infrastructure.Features.Todos.Queries;
using Infrastructure.Middleware;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Infrastructure;

public static class DependencyInjection
{
    public const string PersonIdItemKey = "HomeApp.PersonId";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddDatabase(configuration)
            .AddCors()
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration);

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IPersonIdCache, PersonIdCache>();

        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

        services.AddScoped<IPersonValidation, PersonValidation>();
        services.AddScoped<IPersonProvisioningService, PersonProvisioningService>();

        services.AddScoped<IBudgetCommands, BudgetCommands>();
        services.AddScoped<IBudgetQueries, BudgetQueries>();
        services.AddScoped<IHouseholdCommands, HouseholdCommands>();
        services.AddScoped<IHouseholdQueries, HouseholdQueries>();
        services.AddScoped<IAccountCommands, AccountCommands>();
        services.AddScoped<IAccountQueries, AccountQueries>();
        services.AddScoped<ICategoryCommands, CategoryCommands>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<ITransactionCommands, TransactionCommands>();
        services.AddScoped<ITransactionQueries, TransactionQueries>();
        services.AddScoped<IPersonCommands, PersonCommands>();
        services.AddScoped<IPersonQueries, PersonQueries>();
        services.AddScoped<ITodoCommands, TodoCommands>();
        services.AddScoped<ITodoQueries, TodoQueries>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HomeAppConnection");

        services.AddDbContext<HomeAppContext>(options =>
            options.UseNpgsql(
                    connectionString,
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable("__ef_migrations_homeapp", "public");
                    })
                .UseSnakeCaseNamingConvention());

        return services;
    }

    private static IServiceCollection AddCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", policy => policy
                .WithOrigins("http://localhost:4200", "https://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        return services;
    }

    private static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRequestTimeouts(
            configure: static timeouts =>
                timeouts.AddPolicy("HealthChecks", TimeSpan.FromSeconds(5)));

        services.AddOutputCache(
            configureOptions: static caching =>
                caching.AddPolicy("HealthChecks",
                    build: static policy => policy.Expire(TimeSpan.FromSeconds(10))));

        services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddNpgSql(configuration.GetConnectionString("HomeAppConnection"), name: "postgres");

        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        var authority = configuration["OAuth:Authority"];
        var validAudiences = configuration.GetSection("OAuth:ValidAudiences").Get<string[]>() ?? [];

        if (string.IsNullOrWhiteSpace(authority) || validAudiences.Length == 0)
        {
            throw new InvalidOperationException(
                "OAuth is required. Configure OAuth:Authority and OAuth:ValidAudiences.");
        }

        services.Configure<OAuthOptions>(configuration.GetSection(OAuthOptions.SectionName));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.RequireHttpsMetadata = false;
                options.MapInboundClaims = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudiences = validAudiences,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidAlgorithms = ["RS256", "ES256", "EdDSA"],
                    ValidTypes = ["at+jwt"],
                    NameClaimType = "sub",
                    RoleClaimType = "roles"
                };
            });

        services.AddAuthorization();
        services.AddScoped<IExecutionContextAccessor, ExecutionContextAccessor>();

        return services;
    }

    public static IHostApplicationBuilder AddInfrastructureTelemetry(
        this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var environment = builder.Environment;
        var telemetrySection = builder.Configuration.GetSection("Telemetry");
        var otlpEndpoint = telemetrySection["Exporter:Otlp:Endpoint"];

        var serviceName = configuration["Telemetry:ServiceName"] ?? "HomeApp.Api";

        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService(serviceName)
            .AddAttributes(new[]
            {
                new KeyValuePair<string, object>("deployment.environment", environment.EnvironmentName)
            });

        var useConsole = bool.TryParse(telemetrySection["Exporter:UseConsole"], out var consoleEnabled) &&
                         consoleEnabled;

        var useOtlpExporter = !string.IsNullOrWhiteSpace(otlpEndpoint);

        builder.Logging.ClearProviders();

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        "HomeApp.Api")
                    .SetResourceBuilder(resourceBuilder);

                if (useOtlpExporter)
                {
                    metrics.AddOtlpExporter(opt => opt.Endpoint = new Uri(otlpEndpoint));
                }

                if (useConsole)
                {
                    metrics.AddConsoleExporter();
                }
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    tracing.SetSampler<AlwaysOnSampler>();
                }

                tracing.AddAspNetCoreInstrumentation(o => { o.RecordException = true; })
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSource("HomeApp.Activity")
                    .SetResourceBuilder(resourceBuilder);

                if (useOtlpExporter)
                {
                    tracing.AddOtlpExporter(opt => opt.Endpoint = new Uri(otlpEndpoint));
                }

                if (useConsole)
                {
                    tracing.AddConsoleExporter();
                }
            });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.SetResourceBuilder(resourceBuilder);
            logging.IncludeScopes = true;
            logging.IncludeFormattedMessage = true;
            logging.ParseStateValues = true;

            if (useOtlpExporter)
            {
                logging.AddOtlpExporter(opt => opt.Endpoint = new Uri(otlpEndpoint));
            }
            else
            {
                logging.AddConsoleExporter();
            }
        });

        return builder;
    }
}
