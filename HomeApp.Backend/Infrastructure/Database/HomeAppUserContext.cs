using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class HomeAppUserContext(DbContextOptions<HomeAppUserContext> options) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" }
        );
    }
}
