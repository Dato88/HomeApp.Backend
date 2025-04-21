using ApplicationTests.IntegrationTests.TestData;
using Infrastructure.Features.Todos.Commands;

namespace ApplicationTests.IntegrationTests.Todos;

public abstract class BaseTodoCommandsTest : BaseTest
{
    protected readonly PeopleDataSeeder PeopleDataSeeder;
    protected readonly TodoCommands TodoCommands;
    protected readonly TodosDataSeeder TodosDataSeeder;

    protected BaseTodoCommandsTest(UnitTestingApiFactory unitTestingApiFactory)
        : base(unitTestingApiFactory)
    {
        PeopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);
        TodosDataSeeder = new TodosDataSeeder(unitTestingApiFactory);
        TodoCommands = new TodoCommands(DbContext);
    }
}
