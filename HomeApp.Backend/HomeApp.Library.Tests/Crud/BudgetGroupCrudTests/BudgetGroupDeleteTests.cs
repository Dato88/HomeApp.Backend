namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests
{
    public class BudgetGroupDeleteTests : BaseBudgetGroupTest
    {
        [Fact]
        public async Task DeleteAsync_RemovesBudgetGroupFromContext()
        {
            // Arrange
            BudgetGroup budgetGroup = new()
            {
                Index = 1,
                Name = "Test Budget Group"
            };

            _context.BudgetGroups.Add(budgetGroup);
            await _context.SaveChangesAsync();

            // Act
            await _budgetGroupCrud.DeleteAsync(budgetGroup.Id);

            // Assert
            _context.BudgetGroups.Should().NotContain(budgetGroup);
        }

        [Fact]
        public async Task DeleteAsync_ThrowsException_WhenBudgetGroupNotFound()
        {
            // Act
            Func<Task> action = async () => await _budgetGroupCrud.DeleteAsync(999);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                                 .WithMessage(BudgetMessage.GroupNotFound);
        }
    }
}
