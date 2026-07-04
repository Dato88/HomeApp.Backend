using Domain.Entities.Budgets;
using Infrastructure.Features.Budgets.Commands;
using Infrastructure.Features.Budgets.Queries;

namespace ApplicationTests.IntegrationTests.Households;

public class SharedBudgetTests : BaseHouseholdCommandsTest
{
    public SharedBudgetTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task SharedHousehold_PartnerCanSeeAndEditBudget()
    {
        // Arrange
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId,
            partner.PersonId);

        var ownCommands = new BudgetCommands(DbContext, ExecutionContext);
        var createResult = await ownCommands.CreateBudgetAsync(household.HouseholdId, 2031, CancellationToken.None);
        createResult.IsSuccess.Should().BeTrue();

        var partnerContext = MockExecutionContext(partner.PersonId);
        var partnerCommands = new BudgetCommands(DbContext, partnerContext);
        var partnerQueries = new BudgetQueries(DbContext, partnerContext);

        // Act
        var readResult = await partnerQueries.GetBudgetAsync(household.HouseholdId, 2031, CancellationToken.None);
        var updateResult = await partnerCommands.UpdateBudgetAsync(createResult.Value, 2032,
            CancellationToken.None);

        // Assert
        readResult.IsSuccess.Should().BeTrue();
        readResult.Value.BudgetId.Should().Be(createResult.Value);
        updateResult.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task SharedHousehold_NonMemberCannotSeeBudget()
    {
        // Arrange
        var partner = await PeopleDataSeeder.SeedPersonAsync();
        var stranger = await PeopleDataSeeder.SeedPersonAsync();
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId,
            partner.PersonId);

        var ownCommands = new BudgetCommands(DbContext, ExecutionContext);
        var createResult = await ownCommands.CreateBudgetAsync(household.HouseholdId, 2031, CancellationToken.None);
        createResult.IsSuccess.Should().BeTrue();

        var strangerContext = MockExecutionContext(stranger.PersonId);
        var strangerQueries = new BudgetQueries(DbContext, strangerContext);
        var strangerCommands = new BudgetCommands(DbContext, strangerContext);

        // Act
        var readResult = await strangerQueries.GetBudgetAsync(household.HouseholdId, 2031, CancellationToken.None);
        var deleteResult = await strangerCommands.DeleteBudgetAsync(createResult.Value, CancellationToken.None);

        // Assert
        readResult.IsFailure.Should().BeTrue();
        deleteResult.IsFailure.Should().BeTrue();
        deleteResult.Error.Should().Be(BudgetErrors.DeleteFailed(createResult.Value));
    }
}
