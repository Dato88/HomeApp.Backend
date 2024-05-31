namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests
{
    public class BudgetRowReadTests : BaseBudgetRowTest
    {
        [Fact]
        public
            async Task FindByIdAsync_ReturnsBudgetRow_WhenExists()
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
            BudgetRow? result = await _budgetRowCrud.FindByIdAsync(budgetRow.Id);

            // Assert
            result.Should().Be(budgetRow);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Assert
            Func<Task> action = async () => await _budgetRowCrud.FindByIdAsync(999);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                                 .WithMessage(BudgetMessage.CellNotFound);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBudgetRows()
        {
            // Arrange
            BudgetRow budgetRow1 = new()
            {
                Index = 1,
                Name = "Test Budget Row 1"
            };

            BudgetRow budgetRow2 = new()
            {
                Index = 2,
                Name = "Test Budget Row 2"
            };

            _context.BudgetRows.Add(budgetRow1);
            _context.BudgetRows.Add(budgetRow2);
            await _context.SaveChangesAsync();

            // Act
            IEnumerable<BudgetRow>? result = await _budgetRowCrud.GetAllAsync();

            // Assert
            result.Should().Contain(new[] { budgetRow1, budgetRow2 });
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
                new BudgetRow { UserId = 1, Index = 1, Name = "Test Budget Row 1" },
                new BudgetRow { UserId = 2, Index = 2, Name = "Test Budget Row 2" },
                new BudgetRow { UserId = 1, Index = 3, Name = "Test Budget Row 3" },
                new BudgetRow { UserId = 3, Index = 4, Name = "Test Budget Row 4" },
                new BudgetRow { UserId = 2, Index = 5, Name = "Test Budget Row 5" },
                new BudgetRow { UserId = 2, Index = 6, Name = "Test Budget Row 6" },
                new BudgetRow { UserId = 1, Index = 7, Name = "Test Budget Row 7" },
                new BudgetRow { UserId = 3, Index = 8, Name = "Test Budget Row 8" },
                new BudgetRow { UserId = 1, Index = 9, Name = "Test Budget Row 9" },
                new BudgetRow { UserId = 2, Index = 10, Name = "Test Budget Row 10" },
                new BudgetRow { UserId = 1, Index = 11, Name = "Test Budget Row 11" },
                new BudgetRow { UserId = 3, Index = 12, Name = "Test Budget Row 12" }
            };

            _context.BudgetRows.AddRange(budgetRows);

            await _context.SaveChangesAsync();

            IQueryable<BudgetRow>? expectedRows = _context.BudgetRows.Where(x => x.UserId == selectedUserId);

            // Act
            IEnumerable<BudgetRow>? result = await _budgetRowCrud.GetAllAsync(selectedUserId);

            // Assert
            result.Should().HaveCount(selectedCount);
            result.Should().Contain(expectedRows);
        }
    }
}
