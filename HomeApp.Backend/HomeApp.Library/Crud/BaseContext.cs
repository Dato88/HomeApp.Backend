namespace HomeApp.Library.Crud
{
    public class BaseContext(HomeAppContext context)
    {
        protected readonly HomeAppContext _context = context;
    }
}
