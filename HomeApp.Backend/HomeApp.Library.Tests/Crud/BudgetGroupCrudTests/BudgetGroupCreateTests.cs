namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests
{
    public class BudgetGroupCreateTests : BaseBudgetGroupTest
    {
        [Fact]
        public async Task CreateAsync_AddsBudgetGroupToContext()
        {
            // Arrange
            BudgetGroup budgetGroup = new()
            {
                UserId = 1,
                Index = 1,
                Name = "Test Budget Group"
            };

            // Act
            await _budgetGroupCrud.CreateAsync(budgetGroup);

            // Assert
            Assert.Contains(budgetGroup, _context.BudgetGroups);
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenBudgetGroupIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetGroupCrud.CreateAsync(null));
        }

        [Fact]
        public async Task CreateAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetGroup budgetGroup = new()
            {
                UserId = 1,
                Index = 1,
                Name = "Test Budget Group"
            };

            // Act
            await _budgetGroupCrud.CreateAsync(budgetGroup);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateForUserIdAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetGroupIdExistsAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }
    }
}
