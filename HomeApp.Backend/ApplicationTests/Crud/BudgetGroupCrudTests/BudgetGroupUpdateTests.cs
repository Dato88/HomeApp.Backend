namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests;

public class BudgetGroupUpdateTests : BaseBudgetGroupTest
{
    // public BudgetGroupUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task UpdateAsync_UpdatesBudgetGroupInContext()
    // {
    //     // Arrange
    //     BudgetGroup budgetGroup = new() { Index = 0, Name = "Test Budget Group" };
    //
    //     DbContext.BudgetGroups.Add(budgetGroup);
    //     await DbContext.SaveChangesAsync();
    //
    //     BudgetGroup updateBudgetGroup = new() { Id = budgetGroup.Id, Index = 1, Name = "Updated Name" };
    //
    //     // Act
    //     await _budgetGroupCrud.UpdateAsync(updateBudgetGroup, default);
    //
    //     // Assert
    //     budgetGroup.Id.Should().Be(1);
    //     budgetGroup.Index.Should().Be(updateBudgetGroup.Index);
    //     budgetGroup.Name.Should().Be(updateBudgetGroup.Name);
    // }
    //
    // [Fact]
    // public async Task UpdateAsync_ThrowsException_WhenBudgetGroupIsNull() =>
    //     // Act & Assert
    //     await Assert.ThrowsAsync<ArgumentNullException>(async () =>
    //         await _budgetGroupCrud.UpdateAsync(null, default));
    //
    // [Fact]
    // public async Task UpdateAsync_ThrowsException_WhenExistingBudgetGroupIsNull()
    // {
    //     BudgetGroup budgetGroup = new() { Index = 0, Name = "Test Budget Group" };
    //
    //     // Act & Assert
    //     await Assert.ThrowsAsync<InvalidOperationException>(async () =>
    //         await _budgetGroupCrud.UpdateAsync(budgetGroup, default));
    // }
    //
    // [Fact]
    // public async Task UpdateAsync_CallsAllValidations_Once()
    // {
    //     // Arrange
    //     BudgetGroup budgetGroup = new() { Index = 0, Name = "Test Budget Group" };
    //
    //     DbContext.BudgetGroups.Add(budgetGroup);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     await _budgetGroupCrud.UpdateAsync(budgetGroup, default);
    //
    //     // Assert
    //     _budgetValidationMock.Verify(
    //         x => x.ValidateBudgetGroupForUserIdChangeAsync(It.IsAny<int>(), It.IsAny<int>(),
    //             It.IsAny<CancellationToken>()), Times.Once);
    //     _budgetValidationMock.Verify(
    //         x => x.ValidateBudgetGroupIdExistsNotAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    //     _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
    //     _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
    // }
}
