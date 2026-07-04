using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class UpdateBudgetCellTests : BaseBudgetCommandsTest
{
    public UpdateBudgetCellTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Theory]
    [InlineData(1234.56)]
    [InlineData(0)]
    [InlineData(-50)]
    public async Task UpdateBudgetCell_ShouldUpdateAmount(decimal amount)
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], ExecutionContext.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);
        var budgetCell = await BudgetDataSeeder.GenereateDummyBudgetCell(budgetRow.BudgetRowId,
            ExecutionContext.PersonId, 3, 100);

        // Act
        var result = await BudgetCommands.UpdateBudgetCellAsync(budgetCell.BudgetCellId, amount,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.BudgetCells.FindAsync(budgetCell.BudgetCellId);
        updated.Should().NotBeNull();
        updated.Amount.Should().Be(amount);
        updated.UpdatedById.Should().Be(ExecutionContext.PersonId);
        updated.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateBudgetCell_ShouldReturnErrorWhenNotOwned()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], otherPerson.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);
        var budgetCell = await BudgetDataSeeder.GenereateDummyBudgetCell(budgetRow.BudgetRowId,
            otherPerson.PersonId, 3, 100);

        // Act
        var result = await BudgetCommands.UpdateBudgetCellAsync(budgetCell.BudgetCellId, 999,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.UpdateCellFailedWithMessage("BudgetCellId is invalid"));
    }
}
