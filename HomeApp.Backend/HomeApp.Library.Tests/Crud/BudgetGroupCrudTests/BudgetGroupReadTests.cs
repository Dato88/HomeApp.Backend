namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests;

public class BudgetGroupReadTests : BaseBudgetGroupTest
{
    public BudgetGroupReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task FindByIdAsync_ReturnsBudgetGroup_WhenExists()
    {
        // Arrange
        BudgetGroup budgetGroup = new() { Index = 1, Name = "Test Budget Group" };
        DbContext.BudgetGroups.Add(budgetGroup);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetGroupCrud.FindByIdAsync(budgetGroup.Id, default);

        // Assert
        result.Should().BeEquivalentTo(budgetGroup);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _budgetGroupCrud.FindByIdAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task FindByIdAsync_ThrowsException_WhenNotExists()
    {
        // Act
        Func<Task> action = async () => await _budgetGroupCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(BudgetMessage.GroupNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBudgetGroups()
    {
        // Arrange
        var budgetColumns = new[]
        {
            new BudgetGroup { Index = 1, Name = "Test Budget Group 1" },
            new BudgetGroup { Index = 2, Name = "Test Budget Group 2" }
        };

        DbContext.BudgetGroups.AddRange(budgetColumns);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _budgetGroupCrud.GetAllAsync(default);

        // Assert
        result.Should().BeEquivalentTo(budgetColumns);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 4)]
    [InlineData(3, 3)]
    public async Task GetAllAsync_WithUserId_ReturnsAllBudgetGroups(int selectedUserId, int selectedCount)
    {
        // Arrange
        List<BudgetGroup>? budgetGroups = new()
        {
            new BudgetGroup { PersonId = 1, Name = "Test Budget Group 1" },
            new BudgetGroup { PersonId = 2, Name = "Test Budget Group 2" },
            new BudgetGroup { PersonId = 1, Name = "Test Budget Group 3" },
            new BudgetGroup { PersonId = 3, Name = "Test Budget Group 4" },
            new BudgetGroup { PersonId = 2, Name = "Test Budget Group 5" },
            new BudgetGroup { PersonId = 2, Name = "Test Budget Group 6" },
            new BudgetGroup { PersonId = 1, Name = "Test Budget Group 7" },
            new BudgetGroup { PersonId = 3, Name = "Test Budget Group 8" },
            new BudgetGroup { PersonId = 1, Name = "Test Budget Group 9" },
            new BudgetGroup { PersonId = 2, Name = "Test Budget Group 10" },
            new BudgetGroup { PersonId = 1, Name = "Test Budget Group 11" },
            new BudgetGroup { PersonId = 3, Name = "Test Budget Group 12" }
        };

        DbContext.BudgetGroups.AddRange(budgetGroups);

        await DbContext.SaveChangesAsync();

        var expectedGroups = DbContext.BudgetGroups.Where(x => x.PersonId == selectedUserId);

        // Act
        var result = await _budgetGroupCrud.GetAllAsync(selectedUserId, default);

        // Assert
        result.Should().HaveCount(selectedCount);
        result.Should().BeEquivalentTo(expectedGroups);
    }
}
