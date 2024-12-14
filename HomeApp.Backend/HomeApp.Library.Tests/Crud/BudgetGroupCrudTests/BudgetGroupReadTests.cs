namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests;

public class BudgetGroupReadTests : BaseBudgetGroupTest
{
    [Fact]
    public async Task FindByIdAsync_ReturnsBudgetGroup_WhenExists()
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
        var result = await _budgetGroupCrud.FindByIdAsync(budgetGroup.Id, default);

        // Assert
        result.Should().Be(budgetGroup);
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
        BudgetGroup budgetGroup1 = new()
        {
            Index = 1,
            Name = "Test Budget Group 1"
        };

        BudgetGroup budgetGroup2 = new()
        {
            Index = 2,
            Name = "Test Budget Group 2"
        };

        _context.BudgetGroups.Add(budgetGroup1);
        _context.BudgetGroups.Add(budgetGroup2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _budgetGroupCrud.GetAllAsync(default);

        // Assert
        result.Should().Contain(new[] { budgetGroup1, budgetGroup2 });
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
            new BudgetGroup { UserId = 1, Name = "Test Budget Group 1" },
            new BudgetGroup { UserId = 2, Name = "Test Budget Group 2" },
            new BudgetGroup { UserId = 1, Name = "Test Budget Group 3" },
            new BudgetGroup { UserId = 3, Name = "Test Budget Group 4" },
            new BudgetGroup { UserId = 2, Name = "Test Budget Group 5" },
            new BudgetGroup { UserId = 2, Name = "Test Budget Group 6" },
            new BudgetGroup { UserId = 1, Name = "Test Budget Group 7" },
            new BudgetGroup { UserId = 3, Name = "Test Budget Group 8" },
            new BudgetGroup { UserId = 1, Name = "Test Budget Group 9" },
            new BudgetGroup { UserId = 2, Name = "Test Budget Group 10" },
            new BudgetGroup { UserId = 1, Name = "Test Budget Group 11" },
            new BudgetGroup { UserId = 3, Name = "Test Budget Group 12" }
        };

        _context.BudgetGroups.AddRange(budgetGroups);

        await _context.SaveChangesAsync();

        var expectedGroups = _context.BudgetGroups.Where(x => x.UserId == selectedUserId);

        // Act
        var result = await _budgetGroupCrud.GetAllAsync(selectedUserId, default);

        // Assert
        result.Should().HaveCount(selectedCount);
        result.Should().Contain(expectedGroups);
    }
}
