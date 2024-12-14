namespace HomeApp.Library.Tests.Crud.BudgetColumnCrudTests;

public class BudgetColumnReadTests : BaseBudgetColumnTest
{
    [Fact]
    public async Task FindByIdAsync_ReturnsBudgetColumn_WhenExists()
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
        var result = await _budgetColumnCrud.FindByIdAsync(budgetColumn.Id, default);

        // Assert
        result.Should().Be(budgetColumn);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _budgetColumnCrud.FindByIdAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await _budgetColumnCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(BudgetMessage.ColumnNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBudgetColumns()
    {
        // Arrange
        BudgetColumn budgetColumn1 = new()
        {
            Index = 1,
            Name = "Test Budget Column1"
        };

        BudgetColumn budgetColumn2 = new()
        {
            Index = 2,
            Name = "Test Budget Column2"
        };

        _context.BudgetColumns.Add(budgetColumn1);
        _context.BudgetColumns.Add(budgetColumn2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _budgetColumnCrud.GetAllAsync(default);

        // Assert
        result.Should().Contain(new[] { budgetColumn1, budgetColumn2 });
    }
}
