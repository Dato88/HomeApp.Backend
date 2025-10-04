using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;

namespace ApplicationTests.IntegrationTests.Budgets.Commands;

public class CreateBudgetGroupTests : BaseBudgetCommandsTest
{
    public CreateBudgetGroupTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Theory]
    [InlineData(0, "Test Budget Group Income", BudgetGroupType.Income)]
    [InlineData(5, "Test Budget Group Expense", BudgetGroupType.Expense)]
    [InlineData(7, "Test Budget Group Unknown", BudgetGroupType.Unknown)]
    public async Task CreateBudgetGroup_ShouldCreateNewBudgetGroup(int index, string name,
        BudgetGroupType budgetGroupType)
    {
        // Arrange
        var newBudget = await BudgetDataSeeder.GenereateDummyBudget(UserContext.PersonId);

        var newBudgetGroup = new BudgetGroup
        {
            BudgetId = newBudget.BudgetId, Index = index, Title = name, BudgetGroupType = budgetGroupType
        };

        // Act
        var result = await BudgetCommands.CreateBudgetGroupAsync(newBudgetGroup, CancellationToken.None);

        // Assert
        var created = await DbContext.BudgetGroups.FindAsync(result.Value);
        created.Should().NotBeNull();
        created.Index.Should().Be(newBudgetGroup.Index);
        created.Title.Should().Be(newBudgetGroup.Title);
        created.BudgetGroupType.Should().Be(newBudgetGroup.BudgetGroupType);
    }

    [Fact]
    public async Task CreateBudgetGroup_ShouldReturnErrorWhenBudgetIdIsInvalid()
    {
        // Arrange
        var newBudgetGroup = new BudgetGroup { BudgetId = 0, Index = 0, Title = "Test Budget Group" };

        // Act
        var result = await BudgetCommands.CreateBudgetGroupAsync(newBudgetGroup, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.CreateFailedWithMessage("BudgetId is invalid"));
    }
}
