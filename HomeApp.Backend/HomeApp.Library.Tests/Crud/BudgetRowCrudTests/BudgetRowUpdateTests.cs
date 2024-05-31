namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests
{
    public class BudgetRowUpdateTests : BaseBudgetRowTest
    {
        [Fact]
        public async Task UpdateAsync_UpdatesBudgetRowInContext()
        {
            // Arrange
            BudgetRow budgetRow = new()
            {
                Index = 1,
                Name = "Test Budget Row"
            };

            _context.BudgetRows.Add(budgetRow);
            await _context.SaveChangesAsync();

            BudgetRow updateBudgetRow = new()
            {
                Id = budgetRow.Id,
                Index = 2,
                Name = "Updated Budget Row"
            };

            // Act
            await _budgetRowCrud.UpdateAsync(updateBudgetRow);

            // Assert
            budgetRow.Id.Should().Be(1);
            budgetRow.Index.Should().Be(updateBudgetRow.Index);
            budgetRow.Name.Should().Be(updateBudgetRow.Name);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenBudgetRowIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _budgetRowCrud.UpdateAsync(null));
        }

        [Fact]
        public async Task UpdateAsync_ThrowsException_WhenExistingBudgetRowIsNull()
        {
            BudgetRow budgetRow = new()
            {
                Index = 1,
                Name = "Test Budget Row"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _budgetRowCrud.UpdateAsync(budgetRow));
        }

        [Fact]
        public async Task UpdateAsync_CallsAllValidations_Once()
        {
            // Arrange
            BudgetRow budgetRow = new()
            {
                Index = 1,
                Name = "Test Budget Row"
            };

            _context.BudgetRows.Add(budgetRow);

            await _context.SaveChangesAsync();

            // Act
            await _budgetRowCrud.UpdateAsync(budgetRow);

            // Assert
            _budgetValidationMock.Verify(x => x.ValidateBudgetRowForUserIdChangeAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateBudgetRowIdExistsNotAsync(It.IsAny<int>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForEmptyStringAsync(It.IsAny<string>()), Times.Once);
            _budgetValidationMock.Verify(x => x.ValidateForPositiveIndexAsync(It.IsAny<int>()), Times.Once);
        }
    }
}
