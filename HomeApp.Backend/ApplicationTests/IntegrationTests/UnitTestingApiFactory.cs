using Application.Abstractions.Data;
using Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.PostgreSql;

namespace ApplicationTests.IntegrationTests;

public class UnitTestingApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("testdb")
        .WithUsername("testuser")
        .WithPassword("780234iicx5213").Build();

    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    public async Task DisposeAsync() => await _dbContainer.StopAsync();

    protected override void ConfigureWebHost(IWebHostBuilder builder) =>
        builder.ConfigureTestServices(services =>
        {
            // Bestehende Registrierung entfernen
            services.RemoveAll<DbContextOptions<HomeAppContext>>();
            services.RemoveAll<IHomeAppContext>();

            // HomeAppContext mit PostgreSQL (Testcontainer) registrieren
            services.AddDbContext<HomeAppContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));

            // Interface (IHomeAppContext) zur DI hinzufügen
            services.AddScoped<IHomeAppContext>(provider => provider.GetRequiredService<HomeAppContext>());

            // Migrationen anwenden und ggf. initiale Testdaten seeden
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HomeAppContext>();
            context.Database.Migrate();

            // Optional: SeedTestData(context);
        });
}
