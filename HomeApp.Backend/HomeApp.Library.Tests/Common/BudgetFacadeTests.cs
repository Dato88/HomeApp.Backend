namespace HomeApp.Library.Tests.Common;

public class BudgetFacadeTests : BaseBudgetFacadeTest
{
    // public BudgetFacadeTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task GetBudgetAsync_CallsAllValidations_Once()
    // {
    //     // Arrange
    //     _personFacadeMock.Setup(x => x.GetUserPersonAsync(It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(new PersonDto(1, null, "testFirstName", "testLastName", "test@email.com"));
    //
    //     // Act
    //     await _budgetFacade.GetBudgetAsync(default);
    //
    //     // Assert
    //     _budgetCellCrudMock.Verify(
    //         x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    //     _budgetColumnCrudMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    //     _budgetGroupCrudMock.Verify(
    //         x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    //     _budgetRowCrudMock.Verify(
    //         x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    // }
    //
    // [Theory]
    // [InlineData(1)]
    // [InlineData(2)]
    // [InlineData(33)]
    // [InlineData(1234)]
    // public async Task GetBudgetAsync_CallsAllValidations_Once_WithSelectedUserId(int userId)
    // {
    //     // Arrange
    //     var selectedUserId = userId;
    //
    //     _personFacadeMock.Setup(x => x.GetUserPersonAsync(It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(new PersonDto(selectedUserId, null, "testFirstName", "testLastName", "test@email.com"));
    //
    //     // Act
    //     await _budgetFacade.GetBudgetAsync(default);
    //
    //     // Assert
    //     _budgetCellCrudMock.Verify(
    //         x => x.GetAllAsync(selectedUserId, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    //     _budgetColumnCrudMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    //     _budgetGroupCrudMock.Verify(
    //         x => x.GetAllAsync(selectedUserId, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    //     _budgetRowCrudMock.Verify(
    //         x => x.GetAllAsync(selectedUserId, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()),
    //         Times.Once);
    // }
    //
    // [Fact]
    // public async Task GetBudgetAsync_ReturnsBudget_NotNull()
    // {
    //     // Arrange
    //     _personFacadeMock.Setup(x => x.GetUserPersonAsync(It.IsAny<CancellationToken>()))
    //         .ReturnsAsync(new PersonDto());
    //
    //     // Act
    //     var output = await _budgetFacade.GetBudgetAsync(default);
    //
    //     // Assert
    //     output.Should().NotBe(null);
    // }
    //
    // [Fact]
    // public async Task GetBudgetAsync_ReturnsBudget_BudgetCellsNotNull()
    // {
    //     // Arrange
    //     IEnumerable<BudgetCell> budgetCells = new List<BudgetCell>();
    //
    //     _budgetCellCrudMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), default))
    //         .ReturnsAsync(budgetCells);
    //
    //     // Act
    //     var output = await _budgetFacade.GetBudgetAsync(default);
    //
    //     // Assert
    //     output.BudgetCells.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task GetBudgetAsync_ReturnsBudget_BudgetColumnsNotNull()
    // {
    //     // Arrange
    //     IEnumerable<BudgetColumn> budgetColumn = new List<BudgetColumn>();
    //
    //     _budgetColumnCrudMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>(), default))
    //         .ReturnsAsync(budgetColumn);
    //
    //     // Act
    //     var output = await _budgetFacade.GetBudgetAsync(default);
    //
    //     // Assert
    //     output.BudgetColumns.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task GetBudgetAsync_ReturnsBudget_BudgetGroupsNotNull()
    // {
    //     // Arrange
    //     IEnumerable<BudgetGroup> budgetGroup = new List<BudgetGroup>();
    //
    //     _budgetGroupCrudMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), default))
    //         .ReturnsAsync(budgetGroup);
    //
    //     // Act
    //     var output = await _budgetFacade.GetBudgetAsync(default);
    //
    //     // Assert
    //     output.BudgetGroups.Should().NotBeNull();
    // }
    //
    // [Fact]
    // public async Task GetBudgetAsync_ReturnsBudget_BudgetRowsNotNull()
    // {
    //     // Arrange
    //     IEnumerable<BudgetRow> budgetRow = new List<BudgetRow>();
    //
    //     _budgetRowCrudMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>(), default))
    //         .ReturnsAsync(budgetRow);
    //
    //     // Act
    //     var output = await _budgetFacade.GetBudgetAsync(default);
    //
    //     // Assert
    //     output.BudgetRows.Should().NotBeNull();
    // }
}
