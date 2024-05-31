namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests
{
    public class BudgetRowDeleteTests : BaseBudgetRowTest
    {
        [Fact]
        public async Task DeleteAsync_RemovesBudgetRowFromContext()
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
            await _budgetRowCrud.DeleteAsync(budgetRow.Id);

            // Assert
            _context.BudgetRows.Should().NotContain(budgetRow);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenBudgetRowNotFound()
        {
            // Act
            Func<Task> action = async () => await _budgetRowCrud.DeleteAsync(999);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                                 .WithMessage(BudgetMessage.RowNotFound);
        }
    }
}
