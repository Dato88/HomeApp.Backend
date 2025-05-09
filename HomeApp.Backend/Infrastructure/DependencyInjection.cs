﻿using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Features.People.Commands;
using Application.Features.People.Queries;
using Application.Features.People.Validations;
using Application.Features.Todos.Commands;
using Application.Features.Todos.Queries;
using Domain.Entities.User;
using Infrastructure.Database;
using Infrastructure.Features.People.Commands;
using Infrastructure.Features.People.Queries;
using Infrastructure.Features.People.Validations;
using Infrastructure.Features.Todos.Commands;
using Infrastructure.Features.Todos.Queries;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Authorization.Utilities;
using Infrastructure.Services.Logger;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddDatabase(configuration)
            .AddCors()
            .AddHealthChecks(configuration)
            .AddAuthenticationInternal(configuration)
            .AddAuthorizationInternal();

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        ILogger logger =
            new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

        services.AddSingleton(logger);

        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

        services.AddScoped<IPersonValidation, PersonValidation>();

        services.AddScoped<IPersonCommands, PersonCommands>();
        services.AddScoped<IPersonQueries, PersonQueries>();
        services.AddScoped<ITodoCommands, TodoCommands>();
        services.AddScoped<ITodoQueries, TodoQueries>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HomeAppContext>(options =>
            options.UseNpgsql(
                    configuration.GetConnectionString("HomeAppConnection"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable("__ef_migrations_homeapp", "public");
                    })
                .UseSnakeCaseNamingConvention());

        services.AddDbContext<HomeAppUserContext>(options =>
            options.UseNpgsql(
                    configuration.GetConnectionString("HomeAppUserConnection"),
                    npgsqlOptions =>
                    {
                        npgsqlOptions.MigrationsHistoryTable("__ef_migrations_user", "public");
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
        services
            .AddHealthChecks()
            .AddNpgSql(configuration.GetConnectionString("HomeAppConnection")!);
        return services;
    }

    private static IServiceCollection AddAuthenticationInternal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // var jwtSettingsSection = configuration.GetSection("JwtSettings");
        // var jwtSettings = jwtSettingsSection.Get<JwtSettings>()!;
        //
        // // services.Configure<JwtSettings>(jwtSettingsSection);
        // // services.AddSingleton(jwtSettings);
        //
        // services.AddAuthentication(opt =>
        // {
        //     opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        // }).AddJwtBearer(options =>
        // {
        //     options.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true,
        //         ValidIssuer = jwtSettings.ValidIssuer,
        //         ValidAudience = jwtSettings.ValidAudience,
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
        //             .GetBytes(jwtSettings.SecurityKey))
        //     };
        // });

        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<ITokenProvider, TokenProvider>();

        return services;
    }

    private static IServiceCollection AddAuthorizationInternal(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            foreach (var item in ClaimStore.AllClaims)
                options.AddPolicy($"{item.Type.Replace(" ", "")}Policy", policy => policy.RequireClaim(item.Value));
        });

        services.AddIdentity<User, IdentityRole>(
                opt =>
                {
                    opt.Password.RequiredLength = 7;
                    opt.Password.RequireDigit = false;

                    opt.User.RequireUniqueEmail = true;

                    opt.Lockout.AllowedForNewUsers = true;
                    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                    opt.Lockout.MaxFailedAccessAttempts = 3;
                })
            .AddEntityFrameworkStores<HomeAppUserContext>()
            .AddDefaultTokenProviders();

        services.Configure<DataProtectionTokenProviderOptions>(opt =>
            opt.TokenLifespan = TimeSpan.FromHours(1));

        return services;
    }
}
