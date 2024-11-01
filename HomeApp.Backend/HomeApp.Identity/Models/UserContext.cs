using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeApp.Identity.Models;

public class UserContext(DbContextOptions<UserContext> options) : IdentityDbContext<User>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("identity");
    }
}