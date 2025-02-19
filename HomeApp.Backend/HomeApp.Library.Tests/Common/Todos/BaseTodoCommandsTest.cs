using HomeApp.Library.Common.Todos;
using HomeApp.Library.Tests.Helper.CreateDummyData;

namespace HomeApp.Library.Tests.Common.Todos;

public class BaseTodoCommandsTest : BaseTest
{
    protected readonly CreateDummyPeople CreateDummyPeople;
    protected readonly CreateDummyTodos CreateDummyTodos;
    protected readonly TodoCommands TodoCommands;

    public BaseTodoCommandsTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        CreateDummyPeople = new CreateDummyPeople(unitTestingApiFactory);
        CreateDummyTodos = new CreateDummyTodos(unitTestingApiFactory);
        TodoCommands = new TodoCommands(DbContext);
    }
}
