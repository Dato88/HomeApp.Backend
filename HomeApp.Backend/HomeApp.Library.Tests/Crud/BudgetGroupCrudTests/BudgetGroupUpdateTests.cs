namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests
{
    public class BudgetGroupUpdateTests : BaseBudgetGroupTest
    {
        [Fact]
        public async Task UpdateAsync_UpdatesBudgetGroupInContext()
        {
            // Arrange
            BudgetGroup budgetGroup = new()
            {
                Index = 0,
                Name = "Test Budget Group"
            };

            _context.BudgetGroups.Add(budgetGroup);
            await _context.SaveChangesAsync();

            BudgetGroup updateBudgetGroup = new()
            {
                Id = budgetGroup.Id,
                Index = 1,
                Name = "Updated Name"
            };

            // Act
            await _budgetGroupCrud.UpdateAsync(updateBudgetGroup);

            // Assert
            budgetGroup.Id.Should().Be(1);
            budgetGroup.Index.Should().Be(updateBudgetGroup.Index);
            budgetGroup.Name.Should().Be(updateBudgetGroup.Name);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenBudgetGroupIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetGroupCrud.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenExistingBudgetGroupIsNull()
        {
            BudgetGroup budgetGroup = new()
            {
                Index = 0,
                Name = "Test Budget Group"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _budgetGroupCrud.UpdateAsync(budgetGroup));
        }

        [Fact]
        public async Task UpdateAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetGroup budgetGroup = new()
            {
                Index = 0,
                Name = "Test Budget Group"
            };

            _context.BudgetGroups.Add(budgetGroup);
            await _context.SaveChangesAsync();

            // Act
            await _budgetGroupCrud.UpdateAsync(budgetGroup);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateBudgetGroupForUserIdChangeAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetGroupIdExistsNotAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
