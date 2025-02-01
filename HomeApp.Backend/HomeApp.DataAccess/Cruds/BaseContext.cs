using HomeApp.DataAccess.Models;

namespace HomeApp.DataAccess.Cruds;

public class BaseContext(HomeAppContext context)
{
    protected readonly HomeAppContext _context = context;
}
