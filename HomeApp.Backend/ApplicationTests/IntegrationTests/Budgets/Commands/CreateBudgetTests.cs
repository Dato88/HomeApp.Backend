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

        // Act
        var result = await BudgetCommands.CreateBudgetAsync(year, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var created = await DbContext.Budgets.FindAsync(result.Value);
        created.Should().NotBeNull();
        created!.Year.Should().Be(year);
        created.PersonId.Should().Be(UserContext.PersonId);
    }

    [Fact]
    public async Task CreateBudget_ShouldReturnError_WhenInvalidYear()
    {
        // Arrange
        var newBudget = await BudgetDataSeeder.GenereateDummyBudget(UserContext.PersonId);

        // Act
        var result = await BudgetCommands.CreateBudgetAsync(newBudget.Year, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.CreateFailedWithMessage("Budget Year already exists"));
    }
}
