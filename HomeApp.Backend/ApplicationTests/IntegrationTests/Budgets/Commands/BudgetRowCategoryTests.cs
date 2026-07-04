using ApplicationTests.IntegrationTests.TestData;
using Domain.Entities.Budgets;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class BudgetRowCategoryTests : BaseBudgetCommandsTest
{
    private readonly FinanceDataSeeder _financeDataSeeder;

    public BudgetRowCategoryTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _financeDataSeeder = new FinanceDataSeeder(unitTestingApiFactory);

    [Fact]
    public async Task UpdateBudgetRow_ShouldLinkCategoryOfSameHousehold()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], ExecutionContext.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);
        var category = await _financeDataSeeder.GenereateDummyCategory(budget.HouseholdId,
            ExecutionContext.PersonId);

        // Act
        var result = await BudgetCommands.UpdateBudgetRowAsync(budgetRow.BudgetRowId, budgetRow.Index,
            budgetRow.Title, category.CategoryId, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.BudgetRows.FindAsync(budgetRow.BudgetRowId);
        updated!.CategoryId.Should().Be(category.CategoryId);
    }

    [Fact]
    public async Task UpdateBudgetRow_ShouldReturnErrorWhenCategoryBelongsToOtherHousehold()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var foreignCategory = await _financeDataSeeder.GenereateDummyCategory(foreignHousehold.HouseholdId,
            otherPerson.PersonId);

        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 1)], ExecutionContext.PersonId);
        var budgetRow = await DbContext.BudgetRows.SingleAsync(x => x.BudgetGroup.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.UpdateBudgetRowAsync(budgetRow.BudgetRowId, budgetRow.Index,
            budgetRow.Title, foreignCategory.CategoryId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.UpdateRowFailedWithMessage("CategoryId is invalid"));
    }

    [Fact]
    public async Task UpdateBudgetRow_ShouldReturnErrorWhenCategoryAlreadyMappedInSameBudget()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 2)], ExecutionContext.PersonId);
        var budgetRows = await DbContext.BudgetRows
            .Where(x => x.BudgetGroup.BudgetId == budget.BudgetId)
            .OrderBy(x => x.Index)
            .ToListAsync();
        var category = await _financeDataSeeder.GenereateDummyCategory(budget.HouseholdId,
            ExecutionContext.PersonId);

        var firstLink = await BudgetCommands.UpdateBudgetRowAsync(budgetRows[0].BudgetRowId,
            budgetRows[0].Index, budgetRows[0].Title, category.CategoryId, CancellationToken.None);
        firstLink.IsSuccess.Should().BeTrue();

        // Act
        var result = await BudgetCommands.UpdateBudgetRowAsync(budgetRows[1].BudgetRowId, budgetRows[1].Index,
            budgetRows[1].Title, category.CategoryId, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(
            BudgetErrors.UpdateRowFailedWithMessage("Category is already mapped to another row of this budget"));
    }
}
