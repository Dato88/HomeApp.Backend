using HomeApp.Library.Common.Todos;
using HomeApp.Library.Tests.Helper.CreateDummyData;

namespace HomeApp.Library.Tests.Common.Todos;

public class BaseTodoQueriesTest : BaseTest
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
