namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests
{
    public class BudgetRowCreateTests : BaseBudgetRowTest
    {
        [Fact]
        public async Task CreateAsync_AddsBudgetRowToContext()
        {
            // Arrange
            BudgetRow budgetRow = new()
            {
                UserId = 1,
                Index = 1,
                Name = "Test Budget Row"
            };

            // Act
            await _budgetRowCrud.CreateAsync(budgetRow);

            // Assert
            Assert.Contains(budgetRow, _context.BudgetRows);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenBudgetRowIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetRowCrud.CreateAsync(null));
        }

        [Fact]
        public async Task CreateAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetRow budgetRow = new()
            {
                UserId = 1,
                Index = 1,
                Name = "Test Budget Row"
            };

            // Act
            await _budgetRowCrud.CreateAsync(budgetRow);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateForUserIdAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetRowIdExistsAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
        }
    }
}