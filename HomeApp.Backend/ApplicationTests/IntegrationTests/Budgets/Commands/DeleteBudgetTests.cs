using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class DeleteBudgetTests : BaseBudgetCommandsTest
{
    public DeleteBudgetTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task DeleteBudget_ShouldDeleteBudgetWithChildren()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 2)], ExecutionContext.PersonId);

        // Act
        var result = await BudgetCommands.DeleteBudgetAsync(budget.BudgetId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.Budgets.AnyAsync(x => x.BudgetId == budget.BudgetId)).Should().BeFalse();
        (await DbContext.BudgetGroups.AnyAsync(x => x.BudgetId == budget.BudgetId)).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteBudget_ShouldReturnErrorWhenNotOwned()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudget(otherPerson.PersonId);

        // Act
        var result = await BudgetCommands.DeleteBudgetAsync(budget.BudgetId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.DeleteFailed(budget.BudgetId));
        (await DbContext.Budgets.AnyAsync(x => x.BudgetId == budget.BudgetId)).Should().BeTrue();
    }

    [Fact]
    public async Task DeleteBudgetGroup_ShouldDeleteGroup()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetGroups(1, ExecutionContext.PersonId);
        var budgetGroup = await DbContext.BudgetGroups.SingleAsync(x => x.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.DeleteBudgetGroupAsync(budgetGroup.BudgetGroupId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.BudgetGroups.AnyAsync(x => x.BudgetGroupId == budgetGroup.BudgetGroupId)).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteBudgetGroup_ShouldReturnErrorWhenNotOwned()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudgetGroups(1, otherPerson.PersonId);
        var budgetGroup = await DbContext.BudgetGroups.SingleAsync(x => x.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.DeleteBudgetGroupAsync(budgetGroup.BudgetGroupId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.DeleteGroupFailed(budgetGroup.BudgetGroupId));
    }

    [Fact]
    public async Task DeleteBudgetRow_ShouldDeleteRow()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], ExecutionContext.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.DeleteBudgetRowAsync(budgetRow.BudgetRowId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.BudgetRows.AnyAsync(x => x.BudgetRowId == budgetRow.BudgetRowId)).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteBudgetRow_ShouldReturnErrorWhenNotOwned()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], otherPerson.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.DeleteBudgetRowAsync(budgetRow.BudgetRowId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.DeleteRowFailed(budgetRow.BudgetRowId));
    }

    [Fact]
    public async Task DeleteBudgetCell_ShouldDeleteCell()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], ExecutionContext.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);
        var budgetCell = await BudgetDataSeeder.GenereateDummyBudgetCell(budgetRow.BudgetRowId,
            ExecutionContext.PersonId, 6, 42);

        // Act
        var result = await BudgetCommands.DeleteBudgetCellAsync(budgetCell.BudgetCellId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        (await DbContext.BudgetCells.AnyAsync(x => x.BudgetCellId == budgetCell.BudgetCellId)).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteBudgetCell_ShouldReturnErrorWhenNotOwned()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], otherPerson.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);
        var budgetCell = await BudgetDataSeeder.GenereateDummyBudgetCell(budgetRow.BudgetRowId,
            otherPerson.PersonId, 6, 42);

        // Act
        var result = await BudgetCommands.DeleteBudgetCellAsync(budgetCell.BudgetCellId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.DeleteCellFailed(budgetCell.BudgetCellId));
    }
}
