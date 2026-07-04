using Application.Abstractions.Authentication;
using ApplicationTests.IntegrationTests.TestData;
using Domain.ValueObjects;
using Infrastructure.Features.Households.Commands;
using Infrastructure.Features.Households.Queries;

namespace ApplicationTests.IntegrationTests.Households;

public class BaseHouseholdCommandsTest : BaseTest
{
    protected readonly PeopleDataSeeder PeopleDataSeeder;
    protected readonly HouseholdDataSeeder HouseholdDataSeeder;
    protected readonly HouseholdCommands HouseholdCommands;
    protected readonly HouseholdQueries HouseholdQueries;

    protected BaseHouseholdCommandsTest(UnitTestingApiFactory unitTestingApiFactory) : base(
        unitTestingApiFactory, BuildExecutionContextMock(unitTestingApiFactory).Result)
    {
        PeopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);
        HouseholdDataSeeder = new HouseholdDataSeeder(unitTestingApiFactory);
        HouseholdCommands = new HouseholdCommands(DbContext, ExecutionContext);
        HouseholdQueries = new HouseholdQueries(DbContext, ExecutionContext);
    }

    protected static IExecutionContextAccessor MockExecutionContext(int personId)
    {
        var mock = new Mock<IExecutionContextAccessor>();
        mock.SetupGet(x => x.PersonId).Returns(personId);
        return mock.Object;
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
