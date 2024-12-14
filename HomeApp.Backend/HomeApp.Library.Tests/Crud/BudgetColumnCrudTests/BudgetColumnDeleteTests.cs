namespace HomeApp.Library.Tests.Crud.BudgetColumnCrudTests;

public class BudgetColumnDeleteTests : BaseBudgetColumnTest
{
    [Fact]
    public async Task DeleteAsync_ShouldDeleteBudgetColumn_WhenBudgetColumnExists()
    {
        // Arrange
        BudgetColumn budgetColumn = new()
        {
            Index = 1,
            Name = "Test Budget Column"
        };

        _context.BudgetColumns.Add(budgetColumn);
        await _context.SaveChangesAsync();

        // Act
        var result = await _budgetColumnCrud.DeleteAsync(budgetColumn.Id, default);

        // Assert
        result.Should().BeTrue();
        var deletedBudgetColumn = await _context.BudgetColumns.FindAsync(budgetColumn.Id);
        deletedBudgetColumn.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task DeleteAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _budgetColumnCrud.DeleteAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenBudgetColumnDoesNotExist()
    {
        // Arrange
        var nonExistingBudgetColumnId = 999;

        // Act
        Func<Task> action = async () =>
            await _budgetColumnCrud.DeleteAsync(nonExistingBudgetColumnId, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(BudgetMessage.ColumnNotFound);
    }
}
