using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Database;

// Used only by "dotnet ef" at design time; must mirror the runtime options in DependencyInjection.AddDatabase.
internal sealed class HomeAppContextDesignTimeFactory : IDesignTimeDbContextFactory<HomeAppContext>
{
    public HomeAppContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<HomeAppContext>();

        optionsBuilder
            .UseNpgsql(
                "Host=localhost;Port=5060;Database=homeapp;Username=postgres;Password=postgres",
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable("__ef_migrations_homeapp", "public"))
            .UseSnakeCaseNamingConvention();

        return new HomeAppContext(optionsBuilder.Options);
    }
}
