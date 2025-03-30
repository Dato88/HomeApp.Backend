using Domain.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class UserContext(DbContextOptions<UserContext> options) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");

        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "e47e06f0-25a5-4dd1-a54b-1f20d680ae6d", Name = "Admin", NormalizedName = "ADMIN" }
        );
    }
}
