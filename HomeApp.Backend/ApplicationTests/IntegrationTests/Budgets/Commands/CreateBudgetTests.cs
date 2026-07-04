using Application.Abstractions.Authentication;
using Domain.Entities.Budgets;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class CreateBudgetTests : BaseBudgetCommandsTest
{
    public CreateBudgetTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateBudget_ShouldCreateNewBudget()
    {
        // Arrange
        var year = 2030;
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);

        // Act
        var result = await BudgetCommands.CreateBudgetAsync(household.HouseholdId, year, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var created = await DbContext.Budgets.FindAsync(result.Value);
        created.Should().NotBeNull();
        created!.Year.Should().Be(year);
        created.HouseholdId.Should().Be(household.HouseholdId);
        created.CreatedById.Should().Be(ExecutionContext.PersonId);
    }

    [Fact]
    public async Task CreateBudget_ShouldReturnError_WhenInvalidYear()
    {
        // Arrange
        var newBudget = await BudgetDataSeeder.GenereateDummyBudget(ExecutionContext.PersonId);

        // Act
        var result = await BudgetCommands.CreateBudgetAsync(newBudget.HouseholdId, newBudget.Year,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.CreateFailedWithMessage("Budget Year already exists"));
    }

    [Fact]
    public async Task CreateBudget_ShouldReturnError_WhenNotMemberOfHousehold()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);

        // Act
        var result = await BudgetCommands.CreateBudgetAsync(foreignHousehold.HouseholdId, 2030,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.CreateFailedWithMessage("HouseholdId is invalid"));
    }
}
