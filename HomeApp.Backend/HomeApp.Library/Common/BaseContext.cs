namespace HomeApp.Library.Common;

public class BaseContext(HomeAppContext dbContext)
{
    protected readonly HomeAppContext DbContext = dbContext;
}
