namespace HomeApp.Library.Tests.Crud.BudgetColumnCrudTests
{
    public class BudgetColumnCreateTests : BaseBudgetColumnTest
    {
        [Fact]
        public async Task CreateAsync_AddsBudgetColumnToContext()
        {
            // Arrange
            BudgetColumn budgetColumn = new()
            {
                Index = 1,
                Name = "Test Budget Column"
            };

            // Act
            await _budgetColumnCrud.CreateAsync(budgetColumn);

            // Assert
            Assert.Contains(budgetColumn, _context.BudgetColumns);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenBudgetColumnIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetColumnCrud.CreateAsync(null));
        }

        [Fact]
        public async Task CreateAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetColumn budgetColumn = new()
            {
                Index = 1,
                Name = "Test Budget Column"
            };

            // Act
            await _budgetColumnCrud.CreateAsync(budgetColumn);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateBudgetColumnIdExistsAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetColumnIndexAndNameAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }
    }
}
