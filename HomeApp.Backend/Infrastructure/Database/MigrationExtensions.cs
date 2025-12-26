using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Database;

public static class MigrationExtensions
{
    public static async Task MigrateDatabaseAsync<TContext>(
        this IHost host,
        string contextName,
        CancellationToken cancellationToken = default)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILogger<TContext>>();

        try
        {
            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            logger.LogInformation("Applying migrations for {Context}", contextName);

            await context.Database.MigrateAsync(cancellationToken);

            logger.LogInformation("Migrations applied successfully for {Context}", contextName);
        }
        catch (Exception ex)
        {
            logger.LogCritical(
                ex,
                "An error occurred while migrating the database for {Context}",
                contextName);

            throw;
        }
    }
}
