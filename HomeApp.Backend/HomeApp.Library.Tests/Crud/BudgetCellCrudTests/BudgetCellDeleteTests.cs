namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests
{
    public class BudgetCellDeleteTests : BaseBudgetCellTest
    {
        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            BudgetCell budgetCell = new()
            {
                Name = "Test",
                BudgetRowId = 1,
                BudgetColumnId = 1
            };

            _context.BudgetCells.Add(budgetCell);
            await _context.SaveChangesAsync();

            // Act
            await _budgetCellCrud.DeleteAsync(budgetCell.Id);

            // Assert
            User? deletedUser = await _context.Users.FindAsync(budgetCell.Id);
            deletedUser.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-3)]
        public async Task DeleteAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
        {
            // Act & Assert
            Func<Task> action = async () => await _budgetCellCrud.DeleteAsync(id);

            await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
                                .WithMessage($"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            int nonExistingUserId = 999;

            // Act
            Func<Task> action = async () => await _budgetCellCrud.DeleteAsync(nonExistingUserId);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                                 .WithMessage(BudgetMessage.CellNotFound);
        }
    }
}
