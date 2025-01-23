namespace HomeApp.Library.Tests.Crud.BudgetColumnCrudTests;

public class BudgetColumnReadTests : BaseBudgetColumnTest
{
    public BudgetColumnReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task FindByIdAsync_ReturnsBudgetColumn_WhenExists()
    {
        // Arrange
        BudgetColumn budgetColumn = new() { Index = 1, Name = "Test Budget Column" };

        DbContext.BudgetColumns.Add(budgetColumn);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetColumnCrud.FindByIdAsync(budgetColumn.Id, default);

        // Assert
        result.Should().BeEquivalentTo(budgetColumn);
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
        var budgetColumns = new[]
        {
            new BudgetColumn { Index = 1, Name = "Test Budget Column1" },
            new BudgetColumn { Index = 2, Name = "Test Budget Column2" }
        };

        DbContext.BudgetColumns.AddRange(budgetColumns);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetColumnCrud.GetAllAsync(default);

        // Assert
        result.Should().BeEquivalentTo(budgetColumns);
    }
}
