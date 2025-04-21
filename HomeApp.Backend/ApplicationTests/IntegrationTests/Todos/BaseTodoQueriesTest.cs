using ApplicationTests.IntegrationTests.TestData;
using Infrastructure.Features.Todos.Queries;

namespace ApplicationTests.IntegrationTests.Todos;

public abstract class BaseTodoQueriesTest : BaseTest
{
    protected readonly PeopleDataSeeder PeopleDataSeeder;
    protected readonly TodoQueries TodoQueries;
    protected readonly TodosDataSeeder TodosDataSeeder;

    public BaseTodoQueriesTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        PeopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);
        TodosDataSeeder = new TodosDataSeeder(unitTestingApiFactory);
        TodoQueries = new TodoQueries(DbContext);
    }
}
