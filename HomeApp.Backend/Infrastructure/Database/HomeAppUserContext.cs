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
            new IdentityRole
            {
                Id = "11111111-1111-1111-1111-111111111111",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "11111111-1111-1111-1111-111111111111"
            }
        );
    }
}
