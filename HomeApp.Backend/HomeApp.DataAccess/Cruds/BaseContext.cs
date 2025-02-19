using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds;

public class BaseContext(HomeAppContext dbContext)
{
    protected readonly HomeAppContext DbContext = dbContext;
}
