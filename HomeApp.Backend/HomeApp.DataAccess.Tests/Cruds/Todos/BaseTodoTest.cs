using HomeApp.DataAccess.Cruds.Todos;
using HomeApp.DataAccess.Tests.Helper;

namespace HomeApp.DataAccess.Tests.Cruds.Todos;

public class BaseTodoTest : BaseTest
{
    protected readonly TodoCommands _todoCommands;

    public BaseTodoTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _todoCommands = new TodoCommands(DbContext);
}
