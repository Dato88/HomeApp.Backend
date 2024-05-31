﻿namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests
{
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
                BudgetGroupId = 1
            };

            _context.BudgetCells.Add(budgetCell);
            await _context.SaveChangesAsync();

            BudgetCell updateBudgetCell = new()
            {
                Id = budgetCell.Id,
                Name = "Updated Name",
                BudgetRowId = 2,
                BudgetColumnId = 2,
                BudgetGroupId = 2
            };

            // Act
            await _budgetCellCrud.UpdateAsync(updateBudgetCell);

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
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetCellCrud.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenExistingBudgetCellIsNull()
        {
            BudgetCell budgetCell = new()
            {
                Name = "Initial Name",
                BudgetRowId = 1,
                BudgetColumnId = 1,
                BudgetGroupId = 1
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _budgetCellCrud.UpdateAsync(budgetCell));
        }

        [Fact]
        public async Task UpdateBudgetCellAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetCell budgetCell = new()
            {
                Name = "Test Budget Cell",
                BudgetRowId = 1,
                BudgetColumnId = 1
            };

            _context.BudgetCells.Add(budgetCell);

            _context.SaveChanges();

            // Act
            await _budgetCellCrud.UpdateAsync(budgetCell);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateBudgetCellForUserIdChangeAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetRowIdExistsAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetColumnIdExistsAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetGroupIdExistsAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
