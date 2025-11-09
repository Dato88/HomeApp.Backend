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
        unitTestingApiFactory, BuildUserContextMock(unitTestingApiFactory).Result)
    {
        PeopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);
        BudgetCommands = new BudgetCommands(DbContext, UserContext);
        BudgetDataSeeder = new BudgetDataSeeder(unitTestingApiFactory);
    }

    private static async Task<IUserContext> BuildUserContextMock(UnitTestingApiFactory factory)
    {
        // Eine echte Person in die Test-DB seeden
        var person = await new PeopleDataSeeder(factory).SeedPersonAsync(); // schreibt SaveChanges() by default
        // Mock für IUserContext
        var mock = new Mock<IUserContext>();
        mock.SetupGet(x => x.PersonId).Returns(person.PersonId);
        mock.SetupGet(x => x.UserEmail).Returns(new UserEmail(person.Email));
        mock.SetupGet(x => x.UserId).Returns(Guid.TryParse(person.UserId, out var g) ? g : Guid.NewGuid());
        return mock.Object;
    }
}
