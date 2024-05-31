namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests
{
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
            BudgetGroup? result = await _budgetGroupCrud.FindByIdAsync(budgetGroup.Id);

            // Assert
            result.Should().Be(budgetGroup);
        }

        [Fact]
        public async Task FindByIdAsync_ThrowsException_WhenNotExists()
        {
            // Act
            Func<Task> action = async () => await _budgetGroupCrud.FindByIdAsync(999);

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
            IEnumerable<BudgetGroup> result = await _budgetGroupCrud.GetAllAsync();

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

            IQueryable<BudgetGroup>? expectedGroups = _context.BudgetGroups.Where(x => x.UserId == selectedUserId);

            // Act
            IEnumerable<BudgetGroup>? result = await _budgetGroupCrud.GetAllAsync(selectedUserId);

            // Assert
            result.Should().HaveCount(selectedCount);
            result.Should().Contain(expectedGroups);
        }
    }
}
