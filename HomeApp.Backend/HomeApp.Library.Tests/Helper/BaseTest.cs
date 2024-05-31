namespace HomeApp.Library.Tests.Helper
{
    public class BaseTest
    {
        protected readonly HomeAppContext _context;

        public BaseTest()
        {
            _context = StaticLibraryHelper.CreateInMemoryContext();
        }
    }
}
