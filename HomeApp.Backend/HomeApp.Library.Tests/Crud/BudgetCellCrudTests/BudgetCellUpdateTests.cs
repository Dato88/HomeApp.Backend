using HomeApp.DataAccess.Models;

namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests;

public class BudgetCellUpdateTests : BaseBudgetCellTest
{
    [Fact]
    public async Task UpdateAsync_UpdatesBudgetCellInContext()
    {
        // Arrange
        BudgetCell budgetCell = new()
        {
            Name = "Initial Name",
            BudgetRowId = 1,
            BudgetColumnId = 1,
            BudgetGroupId = 1,
            Year = 2021
        };

        _context.BudgetCells.Add(budgetCell);
        await _context.SaveChangesAsync();

        BudgetCell updateBudgetCell = new()
        {
            Id = budgetCell.Id,
            Name = "Updated Name",
            BudgetRowId = 2,
            BudgetColumnId = 2,
            BudgetGroupId = 2,
            Year = 2021
        };

        // Act
        await _budgetCellCrud.UpdateAsync(updateBudgetCell, default);

        // Assert
        budgetCell.Id.Should().Be(1);
        budgetCell.Name.Should().Be(updateBudgetCell.Name);
        budgetCell.BudgetRowId.Should().Be(updateBudgetCell.BudgetRowId);
        budgetCell.BudgetColumnId.Should().Be(updateBudgetCell.BudgetColumnId);
        budgetCell.BudgetGroupId.Should().Be(updateBudgetCell.BudgetGroupId);
    }

    [Fact]
    public async Task UpdateBudgetCellAsync_ThrowsException_WhenBudgetCellIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _budgetCellCrud.UpdateAsync(null, default));
    }

    [Fact]
    public async Task UpdateBudgetCellAsync_ThrowsException_WhenBudgetYearIsNullOrEmpty()
    {
        // Arrange
        BudgetCell budgetCell = new()
        {
            Name = "Test Budget Cell",
            BudgetRowId = 1,
            BudgetColumnId = 1,
            Year = 2021
        };

        _context.BudgetCells.Add(budgetCell);

        _context.SaveChanges();

        BudgetCell budgetCellUpdate = new()
        {
            Id = budgetCell.Id,
            Name = "Test Budget Cell",
            BudgetRowId = 1,
            BudgetColumnId = 1
        };

        // Act & Assert
        Func<Task> action = async () => await _budgetCellCrud.UpdateAsync(budgetCellUpdate, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"Year ('{budgetCellUpdate.Year}') must be a non-negative and non-zero value. (Parameter 'Year')Actual value was {budgetCellUpdate.Year}.");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenExistingBudgetCellIsNull()
    {
        BudgetCell budgetCell = new()
        {
            Name = "Initial Name",
            BudgetRowId = 1,
            BudgetColumnId = 1,
            BudgetGroupId = 1,
            Year = 2021
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _budgetCellCrud.UpdateAsync(budgetCell, default));
    }

    [Fact]
    public async Task UpdateBudgetCellAsync_CallsAllValidations_Once()
    {
        // Arrange
        BudgetCell budgetCell = new()
        {
            Name = "Test Budget Cell",
            BudgetRowId = 1,
            BudgetColumnId = 1,
            Year = 2021
        };

        _context.BudgetCells.Add(budgetCell);

        _context.SaveChanges();

        // Act
        await _budgetCellCrud.UpdateAsync(budgetCell, default);

        // Assert
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetCellForUserIdChangeAsync(It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<CancellationToken>()), Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetRowIdExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetColumnIdExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        _budgetValidationMock.Verify(
            x => x.ValidateBudgetGroupIdExistsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
