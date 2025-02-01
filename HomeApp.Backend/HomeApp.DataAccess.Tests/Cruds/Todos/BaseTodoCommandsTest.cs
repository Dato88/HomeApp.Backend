using HomeApp.DataAccess.Cruds.Todos;
using HomeApp.DataAccess.Tests.Helper;

namespace HomeApp.DataAccess.Tests.Cruds.Todos;

public class BaseTodoCommandsTest : BaseTest
{
    protected readonly TodoCommands TodoCommands;

    public BaseTodoCommandsTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        TodoCommands = new TodoCommands(DbContext);
}
