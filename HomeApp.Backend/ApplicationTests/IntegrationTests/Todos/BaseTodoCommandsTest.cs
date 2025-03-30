using Application.Common.Todos;
using ApplicationTests.IntegrationTests.Helper.CreateDummyData;

namespace ApplicationTests.IntegrationTests.Todos;

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
