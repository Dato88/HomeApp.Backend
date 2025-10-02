using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class CreateBudgetRowTests : BaseBudgetCommandsTest
{
    public CreateBudgetRowTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Theory]
    [InlineData(1, 0, 0, "Group Row Name")]
    [InlineData(5, 3, 3, "Group Row Name")]
    [InlineData(7, 5, 7, "Group Row Name")]
    public async Task CreateBudgetRow_ShouldCreateNewBudgetRow(int groupCount, int selectedGroupIndex, int index,
        string name)
    {
        // Arrange
        var newBudget = await BudgetDataSeeder.GenereateDummyBudgetGroups(groupCount, UserContext.PersonId);

        var groupIds = await DbContext.BudgetGroups
            .Where(g => g.BudgetId == newBudget.BudgetId)
            .OrderBy(g => g.Index) // oder .OrderBy(g => g.BudgetGroupId)
            .Select(g => g.BudgetGroupId)
            .ToListAsync();

        var newBudgetRow = new BudgetRow { BudgetGroupId = groupIds[selectedGroupIndex], Index = index, Name = name };

        // Act
        var result = await BudgetCommands.CreateBudgetRowAsync(newBudgetRow, CancellationToken.None);

        // Assert
        var created = await DbContext.BudgetRows.FindAsync(result.Value);
        created.Should().NotBeNull();
        created.Index.Should().Be(newBudgetRow.Index);
        created.Name.Should().Be(newBudgetRow.Name);
    }

    [Fact]
    public async Task CreateBudgetRow_ShouldReturnErrorWhenBudgetIdIsInvalid()
    {
        // Arrange
        var newBudgetRow = new BudgetRow { BudgetGroupId = 0, Index = 0, Name = "Test Budget Row" };

        // Act
        var result = await BudgetCommands.CreateBudgetRowAsync(newBudgetRow, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.CreateFailedWithMessage("BudgetGroupId is invalid"));
    }
}
