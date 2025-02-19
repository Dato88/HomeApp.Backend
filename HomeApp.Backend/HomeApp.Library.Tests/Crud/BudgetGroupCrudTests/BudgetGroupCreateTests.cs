namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests;

public class BudgetGroupCreateTests : BaseBudgetGroupTest
{
    // public BudgetGroupCreateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task CreateAsync_AddsBudgetGroupToContext()
    // {
    //     // Arrange
    //     BudgetGroup budgetGroup = new() { PersonId = 1, Index = 1, Name = "Test Budget Group" };
    //
    //     // Act
    //     await _budgetGroupCrud.CreateAsync(budgetGroup, default);
    //
    //     // Assert
    //     Assert.Contains(budgetGroup, DbContext.BudgetGroups);
    // }
    //
    // [Fact]
    // public async Task CreateAsync_ThrowsException_WhenBudgetGroupIsNull() =>
    //     // Act & Assert
    //     await Assert.ThrowsAsync<ArgumentNullException>(async () =>
    //         await _budgetGroupCrud.CreateAsync(null, default));
    //
    // [Fact]
    // public async Task CreateAsync_CallsAllValidations_Once()
    // {
    //     // Arrange
    //     BudgetGroup budgetGroup = new() { PersonId = 1, Index = 1, Name = "Test Budget Group" };
    //
    //     // Act
    //     await _budgetGroupCrud.CreateAsync(budgetGroup, default);
    //
    //     // Assert
    //     _budgetValidationMock.Verify(x => x.ValidateForUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
    //         Times.Once);
    //     _budgetValidationMock.Verify(
    //         x => x.ValidateBudgetGroupIdExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    //     _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
    //     _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
    //     _budgetValidationMock.Verify(
    //         x => x.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>(),
    //             It.IsAny<CancellationToken>()),
    //         Times.Once);
    // }
}
