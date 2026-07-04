using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class UpdateBudgetRowTests : BaseBudgetCommandsTest
{
    public UpdateBudgetRowTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task UpdateBudgetRow_ShouldUpdateFields()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], ExecutionContext.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.UpdateBudgetRowAsync(budgetRow.BudgetRowId, 7, "Updated Row", null,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.BudgetRows.FindAsync(budgetRow.BudgetRowId);
        updated.Should().NotBeNull();
        updated.Index.Should().Be(7);
        updated.Title.Should().Be("Updated Row");
        updated.UpdatedById.Should().Be(ExecutionContext.PersonId);
        updated.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateBudgetRow_ShouldReturnErrorWhenNotOwned()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], otherPerson.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.UpdateBudgetRowAsync(budgetRow.BudgetRowId, 1, "Updated Row", null,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.UpdateRowFailedWithMessage("BudgetRowId is invalid"));
    }

    [Fact]
    public async Task UpdateBudgetRow_ShouldReturnErrorWhenIndexExists()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 2)], ExecutionContext.PersonId);
        var budgetRows = await DbContext.BudgetRows
            .Where(x => x.BudgetGroup.BudgetId == budget.BudgetId)
            .OrderBy(x => x.Index)
            .ToListAsync();

        // Act
        var result = await BudgetCommands.UpdateBudgetRowAsync(budgetRows[1].BudgetRowId, budgetRows[0].Index,
            "Updated Row", null, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.UpdateRowFailedWithMessage("Index already exists"));
    }
}
