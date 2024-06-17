namespace HomeApp.Library.Tests.Crud.BudgetColumnCrudTests
{
    public class BudgetColumnUpdateTests : BaseBudgetColumnTest
    {
        [Fact]
        public async Task UpdateAsync_UpdatesBudgetColumnInContext()
        {
            // Arrange
            BudgetColumn budgetColumn = new()
            {
                Index = 0,
                Name = "Test Budget Column"
            };

            _context.BudgetColumns.Add(budgetColumn);
            await _context.SaveChangesAsync();

            BudgetColumn updateBudgetColumn = new()
            {
                Id = budgetColumn.Id,
                Index = 1,
                Name = "Updated Name"
            };

            // Act
            await _budgetColumnCrud.UpdateAsync(updateBudgetColumn);

            // Assert
            budgetColumn.Id.Should().Be(1);
            budgetColumn.Index.Should().Be(updateBudgetColumn.Index);
            budgetColumn.Name.Should().Be(updateBudgetColumn.Name);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenBudgetColumnIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetColumnCrud.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenExistingBudgetColumnIsNull()
        {
            BudgetColumn budgetColumn = new()
            {
                Index = 0,
                Name = "Test Budget Column"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _budgetColumnCrud.UpdateAsync(budgetColumn));
        }

        [Fact]
        public async Task UpdateAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetColumn budgetColumn = new()
            {
                Index = 0,
                Name = "Test Budget Column"
            };

            _context.BudgetColumns.Add(budgetColumn);
            _context.SaveChanges();

            // Act
            await _budgetColumnCrud.UpdateAsync(budgetColumn);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateBudgetColumnIdExistsNotAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetColumnIndexAndNameExistsAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }
    }
}
