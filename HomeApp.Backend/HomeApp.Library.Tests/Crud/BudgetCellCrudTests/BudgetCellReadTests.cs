namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests;

public class BudgetCellReadTests : BaseBudgetCellTest
{
    public BudgetCellReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task FindByIdAsync_ReturnsBudgetCell_WhenExists()
    {
        // Arrange
        Person person = new()
        {
            FirstName = "TestFirstName", LastName = "TestLastName", Email = "testmail@test.com", UserId = "12345"
        };
        DbContext.People.Add(person);
        await DbContext.SaveChangesAsync();

        BudgetColumn budgetColumn = new() { Name = "Test Budget Cell", Index = 1 };
        DbContext.BudgetColumns.Add(budgetColumn);
        await DbContext.SaveChangesAsync();

        BudgetRow budgetRow = new() { Name = "Test Budget Cell", PersonId = 1, Index = 1, Year = 2000 };
        DbContext.BudgetRows.Add(budgetRow);
        await DbContext.SaveChangesAsync();

        BudgetCell budgetCell = new() { Name = "Test Budget Cell", BudgetRowId = 1, BudgetColumnId = 1 };

        DbContext.BudgetCells.Add(budgetCell);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetCellCrud.FindByIdAsync(budgetCell.Id, default);

        // Assert
        result.Should().BeEquivalentTo(budgetCell);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _budgetCellCrud.FindByIdAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsException_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await _budgetCellCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(BudgetMessage.CellNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBudgetCells()
    {
        // Arrange
        var budgetCells = new[]
        {
            new BudgetCell { Name = "Test Budget Cell", BudgetRowId = 1, BudgetColumnId = 1, PersonId = 1 },
            new BudgetCell { Name = "Test Budget Cell2", BudgetRowId = 2, BudgetColumnId = 2, PersonId = 2 }
        };
        DbContext.BudgetCells.AddRange(budgetCells);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetCellCrud.GetAllAsync(default);

        // Assert
        result.Should().HaveCount(budgetCells.Length);
        result.Should().BeEquivalentTo(budgetCells);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 4)]
    [InlineData(3, 3)]
    public async Task GetAllAsync_WithUserId_ReturnsAllBudgetCells(int selectedUserId, int selectedCount)
    {
        // Arrange
        List<BudgetCell>? budgetCells = new()
        {
            new BudgetCell { PersonId = 1, Name = "Test Budget Cell 1" },
            new BudgetCell { PersonId = 2, Name = "Test Budget Cell 2" },
            new BudgetCell { PersonId = 1, Name = "Test Budget Cell 3" },
            new BudgetCell { PersonId = 3, Name = "Test Budget Cell 4" },
            new BudgetCell { PersonId = 2, Name = "Test Budget Cell 5" },
            new BudgetCell { PersonId = 2, Name = "Test Budget Cell 6" },
            new BudgetCell { PersonId = 1, Name = "Test Budget Cell 7" },
            new BudgetCell { PersonId = 3, Name = "Test Budget Cell 8" },
            new BudgetCell { PersonId = 1, Name = "Test Budget Cell 9" },
            new BudgetCell { PersonId = 2, Name = "Test Budget Cell 10" },
            new BudgetCell { PersonId = 1, Name = "Test Budget Cell 11" },
            new BudgetCell { PersonId = 3, Name = "Test Budget Cell 12" }
        };

        DbContext.BudgetCells.AddRange(budgetCells);

        await DbContext.SaveChangesAsync();

        var expectedCells = DbContext.BudgetCells.Where(x => x.PersonId == selectedUserId);

        // Act
        var result = await _budgetCellCrud.GetAllAsync(selectedUserId, default);

        // Assert
        result.Should().HaveCount(selectedCount);
        result.Should().BeEquivalentTo(expectedCells);
    }
}
