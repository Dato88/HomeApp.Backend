using ApplicationTests.IntegrationTests.Helper.CreateDummyData;
using Infrastructure.Features.Todos.Queries;

namespace ApplicationTests.IntegrationTests.Todos;

public abstract class BaseTodoQueriesTest : BaseTest
{
    protected readonly CreateDummyPeople CreateDummyPeople;
    protected readonly CreateDummyTodos CreateDummyTodos;
    protected readonly TodoQueries TodoQueries;

    public BaseTodoQueriesTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        CreateDummyPeople = new CreateDummyPeople(unitTestingApiFactory);
        CreateDummyTodos = new CreateDummyTodos(unitTestingApiFactory);
        TodoQueries = new TodoQueries(DbContext);
    }
}
