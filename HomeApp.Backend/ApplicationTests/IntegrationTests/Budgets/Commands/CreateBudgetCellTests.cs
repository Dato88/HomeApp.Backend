using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class CreateBudgetCellTests : BaseBudgetCommandsTest
{
    public CreateBudgetCellTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateBudgetCell_ShouldCreateNewBudgetCell()
    {
        // Arrange
        (int budgetGroup, int budgetRowCount)[] groupsAndRows =
            [(budgetGroup: 1, budgetRowCount: 1), (budgetGroup: 2, budgetRowCount: 1)];
        var newBudget = await BudgetDataSeeder.GenereateDummyBudgetRows(groupsAndRows, ExecutionContext.PersonId);

        var rowIds = await DbContext.BudgetRows
            .Where(g => g.BudgetGroup.BudgetId == newBudget.BudgetId)
            .OrderBy(g => g.Index) // oder .OrderBy(g => g.BudgetGroupId)
            .Select(g => g.BudgetRowId)
            .ToListAsync();

        var newBudgetCell = new BudgetCell { BudgetRowId = rowIds[0], Month = 1, Amount = 25 };

        // Act
        var result = await BudgetCommands.CreateBudgetCellAsync(newBudgetCell, CancellationToken.None);

        // Assert
        var created = await DbContext.BudgetCells.FindAsync(result.Value);
        created.Should().NotBeNull();
        created.Month.Should().Be(newBudgetCell.Month);
        created.Amount.Should().Be(newBudgetCell.Amount);
    }

    [Fact]
    public async Task CreateBudgetCell_ShouldReturnErrorWhenBudgetRowIdIsInvalid()
    {
        // Arrange
        var newBudgetCell = new BudgetCell { BudgetRowId = 0, Month = 1, Amount = 25 };

        // Act
        var result = await BudgetCommands.CreateBudgetCellAsync(newBudgetCell, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.CreateFailedWithMessage("BudgetRowId is invalid"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(15)]
    public async Task CreateBudgetCell_ShouldReturnErrorWhenMonthIsInvalid(int month)
    {
        // Arrange
        var newBudgetCell = new BudgetCell { BudgetRowId = 0, Month = month, Amount = 25 };

        // Act
        var result = await BudgetCommands.CreateBudgetCellAsync(newBudgetCell, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.CreateFailedWithMessage("Month should be between 1 and 12"));
    }
}
