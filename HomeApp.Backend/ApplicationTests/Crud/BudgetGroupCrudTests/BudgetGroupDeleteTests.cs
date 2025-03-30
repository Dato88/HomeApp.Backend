namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests;

public class BudgetGroupDeleteTests : BaseBudgetGroupTest
{
    // public BudgetGroupDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task DeleteAsync_RemovesBudgetGroupFromContext()
    // {
    //     // Arrange
    //     BudgetGroup budgetGroup = new() { Index = 1, Name = "Test Budget Group" };
    //
    //     DbContext.BudgetGroups.Add(budgetGroup);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     await _budgetGroupCrud.DeleteAsync(budgetGroup.Id, default);
    //
    //     // Assert
    //     DbContext.BudgetGroups.Should().NotContain(budgetGroup);
    // }
    //
    // [Theory]
    // [InlineData(0)]
    // [InlineData(-3)]
    // public async Task DeleteAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    // {
    //     // Act & Assert
    //     Func<Task> action = async () => await _budgetGroupCrud.DeleteAsync(id, default);
    //
    //     await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
    //         .WithMessage(
    //             $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    // }
    //
    // [Fact]
    // public async Task DeleteAsync_ThrowsException_WhenBudgetGroupNotFound()
    // {
    //     // Act
    //     Func<Task> action = async () => await _budgetGroupCrud.DeleteAsync(999, default);
    //
    //     // Assert
    //     await action.Should().ThrowAsync<InvalidOperationException>()
    //         .WithMessage(BudgetMessage.GroupNotFound);
    // }
}
