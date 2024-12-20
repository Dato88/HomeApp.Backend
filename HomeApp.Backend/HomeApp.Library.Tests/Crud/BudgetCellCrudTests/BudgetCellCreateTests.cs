namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests;

public class BudgetCellCreateTests : BaseBudgetCellTest
{
    [Fact]
    public async Task CreateAsync_AddsBudgetCellToContext()
    {
        // Arrange
        BudgetCell budgetCell = new()
        {
            PersonId = 1,
            Name = "Test Budget Cell",
            BudgetRowId = 1,
            BudgetColumnId = 1,
            Year = 2021
        };

        // Act
        CancellationToken cancellationToken = new();

        await _budgetCellCrud.CreateAsync(budgetCell, cancellationToken);

        // Assert
        Assert.Contains(budgetCell, _context.BudgetCells);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenBudgetCellIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _budgetCellCrud.CreateAsync(null, default));

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenBudgetYearIsNullOrEmpty()
    {
        // Arrange
        BudgetCell budgetCell = new() { PersonId = 1, Name = "Test Budget Cell", BudgetRowId = 1, BudgetColumnId = 1 };

        // Act & Assert
        Func<Task> action = async () => await _budgetCellCrud.CreateAsync(budgetCell, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"Year ('{budgetCell.Year}') must be a non-negative and non-zero value. (Parameter 'Year')Actual value was {budgetCell.Year}.");
    }

    [Fact]
    public async Task CreateAsync_CallsAllValidations_Once()
    {
        // Arrange
        BudgetCell budgetCell = new()
        {
            PersonId = 1,
            Name = "Test Budget Cell",
            BudgetRowId = 1,
            BudgetColumnId = 1,
            Year = 2021
        };

        // Act
        await _budgetCellCrud.CreateAsync(budgetCell, default);

        // Assert
        _budgetValidationMock.Verify(x => x.ValidateForUserIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetRowIdExistsNotAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetColumnIdExistsNotAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetGroupIdExistsNotAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_CallsAllValidations_Once_WithSelectedParameter()
    {
        // Arrange
        BudgetCell budgetCell = new()
        {
            PersonId = 1,
            Name = "Test Budget Cell",
            BudgetRowId = 5,
            BudgetColumnId = 11,
            Year = 2021
        };

        // Act
        await _budgetCellCrud.CreateAsync(budgetCell, default);

        // Assert
        _budgetValidationMock.Verify(
            x => x.ValidateForUserIdAsync(budgetCell.PersonId, It.IsAny<CancellationToken>()), Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetRowIdExistsNotAsync(budgetCell.BudgetRowId, It.IsAny<CancellationToken>()),
            Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetColumnIdExistsNotAsync(budgetCell.BudgetColumnId, It.IsAny<CancellationToken>()),
            Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetGroupIdExistsNotAsync(budgetCell.BudgetGroupId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
