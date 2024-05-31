using Microsoft.EntityFrameworkCore;

namespace HomeApp.Library.Tests.Helper
{
    public class StaticLibraryHelper
    {
        public static HomeAppContext CreateInMemoryContext()
        {
            DbContextOptions<HomeAppContext>? options = new DbContextOptionsBuilder<HomeAppContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            HomeAppContext context = new(options);

            return context;
        }
    }
}
