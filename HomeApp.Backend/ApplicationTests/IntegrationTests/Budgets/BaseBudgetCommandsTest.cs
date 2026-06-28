using Application.Abstractions.Authentication;
using ApplicationTests.IntegrationTests.TestData;
using Domain.ValueObjects;
using Infrastructure.Features.Budgets.Commands;

namespace ApplicationTests.IntegrationTests.Budgets;

public class BaseBudgetCommandsTest : BaseTest
{
    protected readonly PeopleDataSeeder PeopleDataSeeder;
    protected readonly BudgetCommands BudgetCommands;
    protected readonly BudgetDataSeeder BudgetDataSeeder;

    protected BaseBudgetCommandsTest(UnitTestingApiFactory unitTestingApiFactory) : base(
        unitTestingApiFactory, BuildExecutionContextMock(unitTestingApiFactory).Result)
    {
        PeopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);
        BudgetCommands = new BudgetCommands(DbContext, ExecutionContext);
        BudgetDataSeeder = new BudgetDataSeeder(unitTestingApiFactory);
    }

    private static async Task<IExecutionContextAccessor> BuildExecutionContextMock(UnitTestingApiFactory factory)
    {
        var person = await new PeopleDataSeeder(factory).SeedPersonAsync();
        var mock = new Mock<IExecutionContextAccessor>();
        mock.SetupGet(x => x.PersonId).Returns(person.PersonId);
        mock.SetupGet(x => x.Email).Returns(new UserEmail(person.Email));
        mock.SetupGet(x => x.KeycloakUserId).Returns(Guid.TryParse(person.UserId, out var g) ? g : Guid.NewGuid());
        return mock.Object;
    }
}
