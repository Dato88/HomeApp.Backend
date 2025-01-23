namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests;

public class BudgetRowReadTests : BaseBudgetRowTest
{
    public BudgetRowReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public
        async Task FindByIdAsync_ReturnsBudgetRow_WhenExists()
    {
        // Arrange
        BudgetRow budgetRow = new() { Index = 1, Name = "Test Budget Row" };

        DbContext.BudgetRows.Add(budgetRow);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetRowCrud.FindByIdAsync(budgetRow.Id, default);

        // Assert
        result.Should().BeEquivalentTo(budgetRow);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _budgetRowCrud.FindByIdAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await _budgetRowCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(BudgetMessage.CellNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBudgetRows()
    {
        // Arrange
        List<BudgetRow>? budgetRows = new()
        {
            new BudgetRow { Index = 1, Name = "Test Budget Row 1" },
            new BudgetRow { Index = 2, Name = "Test Budget Row 2" }
        };

        DbContext.BudgetRows.AddRange(budgetRows);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetRowCrud.GetAllAsync(default);

        // Assert
        result.Should().BeEquivalentTo(budgetRows);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 4)]
    [InlineData(3, 3)]
    public async Task GetAllAsync_WithUserId_ReturnsAllBudgetRows(int selectedUserId, int selectedCount)
    {
        // Arrange
        List<BudgetRow>? budgetRows = new()
        {
            new BudgetRow { PersonId = 1, Index = 1, Name = "Test Budget Row 1" },
            new BudgetRow { PersonId = 2, Index = 2, Name = "Test Budget Row 2" },
            new BudgetRow { PersonId = 1, Index = 3, Name = "Test Budget Row 3" },
            new BudgetRow { PersonId = 3, Index = 4, Name = "Test Budget Row 4" },
            new BudgetRow { PersonId = 2, Index = 5, Name = "Test Budget Row 5" },
            new BudgetRow { PersonId = 2, Index = 6, Name = "Test Budget Row 6" },
            new BudgetRow { PersonId = 1, Index = 7, Name = "Test Budget Row 7" },
            new BudgetRow { PersonId = 3, Index = 8, Name = "Test Budget Row 8" },
            new BudgetRow { PersonId = 1, Index = 9, Name = "Test Budget Row 9" },
            new BudgetRow { PersonId = 2, Index = 10, Name = "Test Budget Row 10" },
            new BudgetRow { PersonId = 1, Index = 11, Name = "Test Budget Row 11" },
            new BudgetRow { PersonId = 3, Index = 12, Name = "Test Budget Row 12" }
        };

        DbContext.BudgetRows.AddRange(budgetRows);

        await DbContext.SaveChangesAsync();

        var expectedRows = DbContext.BudgetRows.Where(x => x.PersonId == selectedUserId);

        // Act
        var result = await _budgetRowCrud.GetAllAsync(selectedUserId, default);

        // Assert
        result.Should().HaveCount(selectedCount);
        result.Should().BeEquivalentTo(expectedRows);
    }
}
