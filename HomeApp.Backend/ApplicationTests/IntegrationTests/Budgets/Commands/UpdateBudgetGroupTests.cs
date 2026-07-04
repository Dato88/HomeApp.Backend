using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class UpdateBudgetGroupTests : BaseBudgetCommandsTest
{
    public UpdateBudgetGroupTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task UpdateBudgetGroup_ShouldUpdateFields()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetGroups(1, ExecutionContext.PersonId);
        var budgetGroup = await DbContext.BudgetGroups.SingleAsync(x => x.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.UpdateBudgetGroupAsync(budgetGroup.BudgetGroupId, 5, "Updated Title",
            BudgetGroupType.Income, 30, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.BudgetGroups.FindAsync(budgetGroup.BudgetGroupId);
        updated.Should().NotBeNull();
        updated.Index.Should().Be(5);
        updated.Title.Should().Be("Updated Title");
        updated.BudgetGroupType.Should().Be(BudgetGroupType.Income);
        updated.TargetPercent.Should().Be(30);
        updated.UpdatedById.Should().Be(ExecutionContext.PersonId);
        updated.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateBudgetGroup_ShouldReturnErrorWhenNotOwned()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudgetGroups(1, otherPerson.PersonId);
        var budgetGroup = await DbContext.BudgetGroups.SingleAsync(x => x.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.UpdateBudgetGroupAsync(budgetGroup.BudgetGroupId, 1, "Updated Title",
            BudgetGroupType.Expense, null, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.UpdateGroupFailedWithMessage("BudgetGroupId is invalid"));
    }

    [Fact]
    public async Task UpdateBudgetGroup_ShouldReturnErrorWhenIndexExists()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetGroups(2, ExecutionContext.PersonId);
        var budgetGroups = await DbContext.BudgetGroups
            .Where(x => x.BudgetId == budget.BudgetId)
            .OrderBy(x => x.Index)
            .ToListAsync();

        // Act
        var result = await BudgetCommands.UpdateBudgetGroupAsync(budgetGroups[1].BudgetGroupId,
            budgetGroups[0].Index, "Updated Title", BudgetGroupType.Expense, null, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.UpdateGroupFailedWithMessage("Index already exists"));
    }

    [Fact]
    public async Task UpdateBudgetGroup_ShouldKeepSameIndex()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetGroups(1, ExecutionContext.PersonId);
        var budgetGroup = await DbContext.BudgetGroups.SingleAsync(x => x.BudgetId == budget.BudgetId);

        // Act
        var result = await BudgetCommands.UpdateBudgetGroupAsync(budgetGroup.BudgetGroupId, budgetGroup.Index,
            "Only Title Changed", budgetGroup.BudgetGroupType, null, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.BudgetGroups.FindAsync(budgetGroup.BudgetGroupId);
        updated.Should().NotBeNull();
        updated.Title.Should().Be("Only Title Changed");
    }
}
