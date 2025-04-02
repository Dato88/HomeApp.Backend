using ApplicationTests.IntegrationTests.Helper.CreateDummyData;
using Infrastructure.Features.Todos.Commands;

namespace ApplicationTests.IntegrationTests.Todos;

public abstract class BaseTodoCommandsTest : BaseTest
{
    protected readonly CreateDummyPeople CreateDummyPeople;
    protected readonly CreateDummyTodos CreateDummyTodos;
    protected readonly TodoCommands TodoCommands;

    protected BaseTodoCommandsTest(UnitTestingApiFactory unitTestingApiFactory)
        : base(unitTestingApiFactory)
    {
        CreateDummyPeople = new CreateDummyPeople(unitTestingApiFactory);
        CreateDummyTodos = new CreateDummyTodos(unitTestingApiFactory);
        TodoCommands = new TodoCommands(DbContext);
    }
}
