namespace HomeApp.Library.Cruds;

public class BaseContext(HomeAppContext context)
{
    protected readonly HomeAppContext _context = context;
}
