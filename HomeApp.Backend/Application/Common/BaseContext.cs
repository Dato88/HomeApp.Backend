using Infrastructure.Database;

namespace Application.Common;

public class BaseContext(HomeAppContext dbContext)
{
    protected readonly HomeAppContext DbContext = dbContext;
}
