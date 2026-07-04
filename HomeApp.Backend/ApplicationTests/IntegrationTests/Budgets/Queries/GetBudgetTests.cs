using Application.Features.Budgets.DTOs;
using Infrastructure.Features.Budgets.Queries;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Budgets.Queries;

public class GetBudgetTests : BaseBudgetCommandsTest
{
    private readonly BudgetQueries _budgetQueries;

    public GetBudgetTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _budgetQueries = new BudgetQueries(DbContext, ExecutionContext);

    [Fact]
    public async Task GetBudget_ShouldReturnBudgetWithGroupType()
    {
        // Arrange
        var budget = await BudgetDataSeeder.GenereateDummyBudgetRows([(0, 2)], ExecutionContext.PersonId);
        var seededGroup = await DbContext.BudgetGroups.SingleAsync(x => x.BudgetId == budget.BudgetId);

        // Act
        var result = await _budgetQueries.GetBudgetAsync(budget.HouseholdId, budget.Year, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.BudgetId.Should().Be(budget.BudgetId);

        var response = (BudgetResponse)result.Value;
        response.BudgetGroups.Should().ContainSingle();
        response.BudgetGroups.Single().BudgetGroupType.Should().Be(seededGroup.BudgetGroupType);
        response.BudgetRows.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetBudget_ShouldNotReturnForeignBudget()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var budget = await BudgetDataSeeder.GenereateDummyBudget(otherPerson.PersonId);

        // Act
        var result = await _budgetQueries.GetBudgetAsync(budget.HouseholdId, budget.Year, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}
