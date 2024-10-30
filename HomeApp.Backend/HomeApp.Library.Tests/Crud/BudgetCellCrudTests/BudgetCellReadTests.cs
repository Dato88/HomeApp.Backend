namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests
{
    public class BudgetCellReadTests : BaseBudgetCellTest
    {
        [Fact]
        public async Task FindByIdAsync_ReturnsBudgetCell_WhenExists()
        {
            // Arrange
            BudgetCell budgetCell = new()
            {
                Name = "Test Budget Cell",
                BudgetRowId = 1,
                BudgetColumnId = 1
            };

            _context.BudgetCells.Add(budgetCell);
            await _context.SaveChangesAsync();

            // Act
            BudgetCell? result = await _budgetCellCrud.FindByIdAsync(budgetCell.Id, default);

            // Assert
            result.Should().Be(budgetCell);
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
            BudgetCell budgetCell1 = new()
            {
                Name = "Test Budget Cell",
                BudgetRowId = 1,
                BudgetColumnId = 1
            };

            BudgetCell budgetCell2 = new()
            {
                Name = "Test Budget Cell2",
                BudgetRowId = 2,
                BudgetColumnId = 2
            };

            _context.BudgetCells.Add(budgetCell1);
            _context.BudgetCells.Add(budgetCell2);
            await _context.SaveChangesAsync();

            // Act
            IEnumerable<BudgetCell>? result = await _budgetCellCrud.GetAllAsync(default);

            // Assert
            result.Should().Contain(new[] { budgetCell1, budgetCell2 });
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
                new BudgetCell { UserId = 1, Name = "Test Budget Cell 1" },
                new BudgetCell { UserId = 2, Name = "Test Budget Cell 2" },
                new BudgetCell { UserId = 1, Name = "Test Budget Cell 3" },
                new BudgetCell { UserId = 3, Name = "Test Budget Cell 4" },
                new BudgetCell { UserId = 2, Name = "Test Budget Cell 5" },
                new BudgetCell { UserId = 2, Name = "Test Budget Cell 6" },
                new BudgetCell { UserId = 1, Name = "Test Budget Cell 7" },
                new BudgetCell { UserId = 3, Name = "Test Budget Cell 8" },
                new BudgetCell { UserId = 1, Name = "Test Budget Cell 9" },
                new BudgetCell { UserId = 2, Name = "Test Budget Cell 10" },
                new BudgetCell { UserId = 1, Name = "Test Budget Cell 11" },
                new BudgetCell { UserId = 3, Name = "Test Budget Cell 12" }
            };

            _context.BudgetCells.AddRange(budgetCells);

            await _context.SaveChangesAsync();

            IQueryable<BudgetCell>? expectedCells = _context.BudgetCells.Where(x => x.UserId == selectedUserId);

            // Act
            IEnumerable<BudgetCell>? result = await _budgetCellCrud.GetAllAsync(selectedUserId, default);

            // Assert
            result.Should().HaveCount(selectedCount);
            result.Should().Contain(expectedCells);
        }
    }
}