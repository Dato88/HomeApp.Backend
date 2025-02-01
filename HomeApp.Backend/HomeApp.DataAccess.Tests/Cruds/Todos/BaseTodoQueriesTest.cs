using HomeApp.DataAccess.Cruds.Todos;
using HomeApp.DataAccess.Tests.Helper;

namespace HomeApp.DataAccess.Tests.Cruds.Todos;

public class BaseTodoQueriesTest : BaseTest
{
    protected readonly TodoQueries TodoQueries;

    public BaseTodoQueriesTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        TodoQueries = new TodoQueries(DbContext);
}
