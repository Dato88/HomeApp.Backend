namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests
{
    public class BudgetCellCreateTests : BaseBudgetCellTest
    {
        [Fact]
        public async Task CreateAsync_AddsBudgetCellToContext()
        {
            // Arrange
            BudgetCell budgetCell = new()
            {
                UserId = 1,
                Name = "Test Budget Cell",
                BudgetRowId = 1,
                BudgetColumnId = 1
            };

            // Act
            await _budgetCellCrud.CreateAsync(budgetCell);

            // Assert
            Assert.Contains(budgetCell, _context.BudgetCells);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenBudgetCellIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetCellCrud.CreateAsync(null));
        }

        [Fact]
        public async Task CreateAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetCell budgetCell = new()
            {
                UserId = 1,
                Name = "Test Budget Cell",
                BudgetRowId = 1,
                BudgetColumnId = 1
            };

            // Act
            await _budgetCellCrud.CreateAsync(budgetCell);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateForUserIdAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetRowIdExistsAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetColumnIdExistsAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetGroupIdExistsAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
