using HomeApp.Library.Models;

namespace HomeApp.Library.Tests.Facades;

public class BudgetFacadeTests : BaseBudgetFacadeTest
{
    [Fact]
    public async Task GetBudgetAsync_CallsAllValidations_Once()
    {
        // Arrange
        var selectedUserId = 1;

        // Act
        await _budgetFacade.GetBudgetAsync(selectedUserId, default);

        // Assert
        _budgetCellCrudMock.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _budgetColumnCrudMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _budgetGroupCrudMock.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _budgetRowCrudMock.Verify(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(33)]
    [InlineData(1234)]
    public async Task GetBudgetAsync_CallsAllValidations_Once_WithSelectedUserId(int userId)
    {
        // Arrange
        var selectedUserId = userId;

        // Act
        await _budgetFacade.GetBudgetAsync(selectedUserId, default);

        // Assert
        _budgetCellCrudMock.Verify(x => x.GetAllAsync(selectedUserId, It.IsAny<CancellationToken>()), Times.Once);
        _budgetColumnCrudMock.Verify(x => x.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        _budgetGroupCrudMock.Verify(x => x.GetAllAsync(selectedUserId, It.IsAny<CancellationToken>()), Times.Once);
        _budgetRowCrudMock.Verify(x => x.GetAllAsync(selectedUserId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetBudgetAsync_ReturnsBudget_NotNull()
    {
        // Arrange
        var selectedUserId = 1;

        // Act
        var output = await _budgetFacade.GetBudgetAsync(selectedUserId, default);

        // Assert
        output.Should().NotBe(null);
    }

    [Fact]
    public async Task GetBudgetAsync_ReturnsBudget_BudgetCellsNotNull()
    {
        // Arrange
        var selectedUserId = 1;
        IEnumerable<BudgetCell> budgetCells = new List<BudgetCell>();

        _budgetCellCrudMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(budgetCells);

        // Act
        var output = await _budgetFacade.GetBudgetAsync(selectedUserId, default);

        // Assert
        output.BudgetCells.Should().NotBeNull();
    }

    [Fact]
    public async Task GetBudgetAsync_ReturnsBudget_BudgetColumnsNotNull()
    {
        // Arrange
        var selectedUserId = 1;
        IEnumerable<BudgetColumn> budgetColumn = new List<BudgetColumn>();

        _budgetColumnCrudMock.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>())).ReturnsAsync(budgetColumn);

        // Act
        var output = await _budgetFacade.GetBudgetAsync(selectedUserId, default);

        // Assert
        output.BudgetColumns.Should().NotBeNull();
    }

    [Fact]
    public async Task GetBudgetAsync_ReturnsBudget_BudgetGroupsNotNull()
    {
        // Arrange
        var selectedUserId = 1;
        IEnumerable<BudgetGroup> budgetGroup = new List<BudgetGroup>();

        _budgetGroupCrudMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(budgetGroup);

        // Act
        var output = await _budgetFacade.GetBudgetAsync(selectedUserId, default);

        // Assert
        output.BudgetGroups.Should().NotBeNull();
    }

    [Fact]
    public async Task GetBudgetAsync_ReturnsBudget_BudgetRowsNotNull()
    {
        // Arrange
        var selectedUserId = 1;
        IEnumerable<BudgetRow> budgetRow = new List<BudgetRow>();

        _budgetRowCrudMock.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(budgetRow);

        // Act
        var output = await _budgetFacade.GetBudgetAsync(selectedUserId, default);

        // Assert
        output.BudgetRows.Should().NotBeNull();
    }
}
