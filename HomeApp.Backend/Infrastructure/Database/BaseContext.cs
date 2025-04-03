namespace Infrastructure.Database;

public class BaseContext(HomeAppContext dbContext)
{
    protected readonly HomeAppContext DbContext = dbContext;
}
