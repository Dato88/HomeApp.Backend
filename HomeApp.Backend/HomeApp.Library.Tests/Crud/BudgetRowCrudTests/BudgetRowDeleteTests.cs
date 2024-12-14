namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests;

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
        await _budgetRowCrud.DeleteAsync(budgetRow.Id, default);

        // Assert
        _context.BudgetRows.Should().NotContain(budgetRow);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task DeleteAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _budgetRowCrud.DeleteAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenBudgetRowNotFound()
    {
        // Act
        Func<Task> action = async () => await _budgetRowCrud.DeleteAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(BudgetMessage.RowNotFound);
    }
}
