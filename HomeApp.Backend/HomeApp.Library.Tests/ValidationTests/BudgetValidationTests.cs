using HomeApp.Library.Validations;

namespace HomeApp.Library.Tests.ValidationTests
{
    public class BudgetValidationTests : BaseTest
    {
        private readonly BudgetValidation _budgetValidation;

        public BudgetValidationTests()
        {
            _budgetValidation = new(_context);
        }

        [Fact]
        public async Task ValidateBudgetCellForUserIdChangeAsync_ValidUserId_And_ValidCellId_NoExceptionThrown()
        {
            // Arrange
            int userId = 1;
            int budgetCellId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(userId, budgetCellId, default);

            await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("BudgetCellId is not zero or negative");
            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetCellId user has not changed");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task ValidateBudgetCellForUserIdChangeAsync_ZeroOrNegativeUserId_And_ValidCellId_NoExceptionThrown(
            int selectedUserId)
        {
            // Arrange
            int userId = selectedUserId;
            int budgetCellId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(userId, budgetCellId, default);

            string output =
                $"{UserMessage.UserId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{UserMessage.UserId}')Actual value was {userId}.";
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("UserId is null or negative")
                .WithMessage(output);

            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetCellId user has not changed");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task ValidateBudgetCellForUserIdChangeAsync_ValidUserId_And_ZeroOrNegativeCellId_NoExceptionThrown(
            int selectedCellId)
        {
            // Arrange
            int userId = 1;
            int budgetCellId = selectedCellId;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(userId, budgetCellId, default);

            string output =
                $"{BudgetMessage.BudgetCellId} ('{budgetCellId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetCellId}')Actual value was {budgetCellId}.";
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("BudgetCellId is null or negative")
                .WithMessage(output);

            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetCellId user has not changed");
        }

        [Fact]
        public async Task ValidateBudgetCellForUserIdChangeAsync_InValidNewUserId_And_ValidCellId_NoExceptionThrown()
        {
            // Arrange
            int userId = 1;

            BudgetCell budgetCell = new()
            {
                UserId = userId,
                Name = "Cell Name"
            };

            _context.BudgetCells.Add(budgetCell);
            await _context.SaveChangesAsync();

            int newUserId = 99;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetCellForUserIdChangeAsync(newUserId, budgetCell.Id, default);

            await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("BudgetCellId is not zero or negative");

            await action.Should().ThrowAsync<InvalidOperationException>("Invalid BudgetColumnId")
                .WithMessage(BudgetMessage.UserChangeNotAllowed);
        }

        [Fact]
        public async Task ValidateBudgetColumnIdExistsAsync_ValidColumnId_NoExceptionThrown()
        {
            // Arrange
            int budgetColumnId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetColumnId, default);

            await action.Should().NotThrowAsync<ArgumentException>("BudgetColumnId is Valid");
            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetColumnId does not exists");
        }

        [Fact]
        public async Task ValidateBudgetColumnIdExistsAsync_ValidColumnId_InvalidOperationExceptionThrown()
        {
            // Arrange
            int budgetColumnId = 1;

            _context.BudgetColumns.Add(new() { Index = 1, Name = "Test" });
            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetColumnIdExistsAsync(budgetColumnId, default);

            await action.Should().ThrowAsync<InvalidOperationException>("BudgetColumnId already Exists")
                .WithMessage(BudgetMessage.ColumnIdExist);
        }

        [Fact]
        public async Task ValidateBudgetColumnIdExistsNotAsync_ValidColumnId_NoExceptionThrown()
        {
            // Arrange
            int budgetColumnId = 1;

            _context.BudgetColumns.Add(new() { Index = 1, Name = "Test" });
            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumnId, default);

            await action.Should().NotThrowAsync<ArgumentException>("BudgetColumnId is Valid");
            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetColumnId does not exists");
        }

        [Fact]
        public async Task ValidateBudgetColumnIdExistsNotAsync_InvalidGroupId_ArgumentExceptionThrown()
        {
            // Arrange
            int budgetColumnId = 0;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumnId, default);

            string output =
                $"{BudgetMessage.BudgetColumnId} ('{budgetColumnId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetColumnId}')Actual value was {budgetColumnId}.";

            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetColumnId").WithMessage(output);
        }

        [Fact]
        public async Task ValidateBudgetColumnIdExistsNotAsync_ValidColumnId_InvalidOperationExceptionThrown()
        {
            // Arrange
            int budgetColumnId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetColumnIdExistsNotAsync(budgetColumnId, default);

            await action.Should().ThrowAsync<InvalidOperationException>("BudgetColumnId does not Exist")
                .WithMessage(BudgetMessage.ColumnIdNotExist);
        }

        [Fact]
        public async Task
            ValidateBudgetColumnIndexAndNameAlreadyExistsAsync_DuplicateIndexAndName_ThrowsInvalidOperationException()
        {
            // Arrange
            int budgetIndex = 1;
            string budgetName = "January";

            _context.BudgetColumns.Add(new() { Index = budgetIndex, Name = budgetName });
            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetColumnIndexAndNameExistsAsync(budgetIndex, budgetName, default);

            await action.Should().ThrowAsync<InvalidOperationException>("BudgetColumn Index and Name already Exist")
                .WithMessage(BudgetMessage.ColumnIndexAlreadyExists);
        }

        [Fact]
        public async Task ValidateBudgetColumnIndexAndNameAlreadyExistsAsync_UniqueIndexAndName_NoExceptionThrown()
        {
            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetColumnIndexAndNameExistsAsync(1, "Unique Column", default);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ValidateBudgetGroupForUserIdChangeAsync_ValidUserId_And_ValidGroupId_NoExceptionThrown()
        {
            // Arrange
            int userId = 1;
            int budgetGroupId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(userId, budgetGroupId, default);

            await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("BudgetGroupId is not zero or negative");
            await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task
            ValidateBudgetGroupForUserIdChangeAsync_ZeroOrNegativeUserId_And_ValidGroupId_NoExceptionThrown(
                int selectedUserId)
        {
            // Arrange
            int userId = selectedUserId;
            int budgetGroupId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(userId, budgetGroupId, default);

            string output =
                $"{UserMessage.UserId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{UserMessage.UserId}')Actual value was {userId}.";
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("UserId is null or negative")
                .WithMessage(output);

            await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task
            ValidateBudgetGroupForUserIdChangeAsync_ValidUserId_And_ZeroOrNegativeGroupId_NoExceptionThrown(
                int selectedGroupId)
        {
            // Arrange
            int userId = 1;
            int budgetGroupId = selectedGroupId;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupForUserIdChangeAsync(userId, budgetGroupId, default);

            string output =
                $"{BudgetMessage.BudgetGroupId} ('{budgetGroupId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetGroupId}')Actual value was {budgetGroupId}.";
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("BudgetGroupId is null or negative")
                .WithMessage(output);

            await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
        }

        [Fact]
        public async Task ValidateBudgetGroupIdExistsAsync_ValidGroupId_NoExceptionThrown()
        {
            // Arrange
            int budgetGroupId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroupId, default);

            await action.Should().NotThrowAsync<ArgumentException>("BudgetGroupId is Valid");
            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetGroupId exists");
        }

        [Fact]
        public async Task ValidateBudgetGroupIdExistsAsync_InvalidGroupId_ArgumentExceptionThrown()
        {
            // Arrange
            int budgetGroupId = 0;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroupId, default);

            string output =
                $"{BudgetMessage.BudgetGroupId} ('{budgetGroupId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetGroupId}')Actual value was {budgetGroupId}.";

            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetGroupId").WithMessage(output);
        }

        [Fact]
        public async Task ValidateBudgetGroupIdExistsAsync_ValidGroupId_InvalidOperationExceptionThrown()
        {
            // Arrange
            int budgetGroupId = 1;

            _context.BudgetGroups.Add(new() { Index = 1, Name = "Test" });
            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIdExistsAsync(budgetGroupId, default);

            await action.Should().ThrowAsync<InvalidOperationException>("BudgetGroupId does Exist already")
                .WithMessage(BudgetMessage.GroupIdExist);
        }

        [Fact]
        public async Task ValidateBudgetGroupIdExistsNotAsync_ValidGroupId_NoExceptionThrown()
        {
            // Arrange
            int budgetGroupId = 1;

            _context.BudgetGroups.Add(new() { Index = 1, Name = "Test" });
            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroupId, default);

            await action.Should().NotThrowAsync<ArgumentException>("BudgetGroupId is Valid");
            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetGroupId exists");
        }

        [Fact]
        public async Task ValidateBudgetGroupIdExistsNotAsync_InvalidGroupId_ArgumentExceptionThrown()
        {
            // Arrange
            int budgetGroupId = 0;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroupId, default);

            string output =
                $"{BudgetMessage.BudgetGroupId} ('{budgetGroupId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetGroupId}')Actual value was {budgetGroupId}.";

            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetGroupId").WithMessage(output);
        }

        [Fact]
        public async Task ValidateBudgetGroupIdExistsNotAsync_GroupDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            int budgetGroupId = 999;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIdExistsNotAsync(budgetGroupId, default);

            await action.Should().ThrowAsync<InvalidOperationException>("BudgetGroupId does not exist")
                .WithMessage(BudgetMessage.GroupIdNotExist);
        }

        [Fact]
        public async Task
            ValidateBudgetGroupIndexAndNameAlreadyExistsAsync_DuplicateIndexAndName_ThrowsInvalidOperationException()
        {
            // Arrange
            BudgetGroup? budgetGroup = new BudgetGroup { Index = 1, Name = "Duplicate Group" };
            _context.BudgetGroups.Add(budgetGroup);
            await _context.SaveChangesAsync();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(budgetGroup.Index,
                    budgetGroup.Name, default);
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(BudgetMessage.GroupIndexAlreadyExists);
        }

        [Fact]
        public async Task ValidateBudgetGroupIndexAndNameAlreadyExistsAsync_UniqueIndexAndName_NoExceptionThrown()
        {
            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(1, "Unique Group", default);
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task ValidateBudgetRowForUserIdChangeAsync_ValidUserId_And_ValidRowId_NoExceptionThrown()
        {
            // Arrange
            int userId = 1;
            int budgetRowId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(userId, budgetRowId, default);

            await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("BudgetRowId is not zero or negative");
            await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task ValidateBudgetRowForUserIdChangeAsync_ZeroOrNegativeUserId_And_ValidRowId_NoExceptionThrown(
            int selectedUserId)
        {
            // Arrange
            int userId = selectedUserId;
            int budgetRowId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(userId, budgetRowId, default);

            string output =
                $"{UserMessage.UserId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{UserMessage.UserId}')Actual value was {userId}.";
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("UserId is null or negative")
                .WithMessage(output);

            await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task ValidateBudgetRowForUserIdChangeAsync_ValidUserId_And_ZeroOrNegativeRowId_NoExceptionThrown(
            int selectedRowId)
        {
            // Arrange
            int userId = 1;
            int budgetRowId = selectedRowId;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowForUserIdChangeAsync(userId, budgetRowId, default);

            string output =
                $"{BudgetMessage.BudgetRowId} ('{budgetRowId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetRowId}')Actual value was {budgetRowId}.";
            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("BudgetRowId is null or negative")
                .WithMessage(output);

            await action.Should().NotThrowAsync<InvalidOperationException>("UserId user has not changed");
        }

        [Fact]
        public async Task ValidateBudgetRowIdExistsAsync_ValidBudgetRowId_NoExceptionThrown()
        {
            // Arrange
            int budgetRowId = 1;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRowId, default);

            await action.Should().NotThrowAsync<ArgumentException>("BudgetRowId is Valid");
            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetRowId exists");
        }

        [Fact]
        public async Task ValidateBudgetRowIdExistsAsync_InvalidBudgetRowId_ArgumentExceptionThrown()
        {
            // Arrange
            int budgetRowId = 0;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRowId, default);

            string output =
                $"{BudgetMessage.BudgetRowId} ('{budgetRowId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetRowId}')Actual value was {budgetRowId}.";

            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetRowId").WithMessage(output);
        }

        [Fact]
        public async Task ValidateBudgetRowIdExistsAsync_BudgetRowDoesExist_ThrowsInvalidOperationException()
        {
            // Arrange
            int budgetRowId = 1;

            _context.BudgetRows.Add(new() { Index = 1, Name = "Test" });
            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowIdExistsAsync(budgetRowId, default);

            await action.Should().ThrowAsync<InvalidOperationException>("BudgetRowId does Exist already")
                .WithMessage(BudgetMessage.RowIdExists);
        }

        [Fact]
        public async Task ValidateBudgetRowIdExistsNotAsync_ValidBudgetRowId_NoExceptionThrown()
        {
            // Arrange
            int budgetRowId = 1;

            _context.BudgetRows.Add(new() { Index = 1, Name = "Test" });
            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRowId, default);

            await action.Should().NotThrowAsync<ArgumentException>("BudgetRowId is Valid");
            await action.Should().NotThrowAsync<InvalidOperationException>("BudgetRowId exists");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task ValidateBudgetRowIdExistsNotAsync_InvalidBudgetRowId_ArgumentExceptionThrown(
            int selectedRowId)
        {
            // Arrange
            int budgetRowId = selectedRowId;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRowId, default);

            string output =
                $"{BudgetMessage.BudgetRowId} ('{budgetRowId}') must be a non-negative and non-zero value. (Parameter '{BudgetMessage.BudgetRowId}')Actual value was {budgetRowId}.";

            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Invalid BudgetRowId").WithMessage(output);
        }

        [Fact]
        public async Task ValidateBudgetRowIdExistsNotAsync_BudgetRowDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            int budgetRowId = 999;

            // Act & Assert
            Func<Task> action = async () =>
                await _budgetValidation.ValidateBudgetRowIdExistsNotAsync(budgetRowId, default);

            await action.Should().ThrowAsync<InvalidOperationException>("BudgetRowId does not exist")
                .WithMessage(BudgetMessage.RowIdNotExist);
        }

        [Fact]
        public async Task ValidateForEmptyString_NoExceptionThrown()
        {
            // Arrange
            string name = "TestName";

            // Act & Assert
            Func<Task> action = async () => await _budgetValidation.ValidateForEmptyStringAsync(name);

            await action.Should().NotThrowAsync<ArgumentException>("Name is not null or empty");
        }

        [Fact]
        public async Task ValidateForEmptyString_EmptyString_ArgumentExceptionThrown()
        {
            // Arrange
            string name = string.Empty;

            // Act & Assert
            Func<Task> action = async () => await _budgetValidation.ValidateForEmptyStringAsync(name);

            string output = $"The value cannot be an empty string. (Parameter 'Budget Name')";

            await action.Should().ThrowAsync<ArgumentException>("Name is empty").WithMessage(output);
        }

        [Fact]
        public async Task ValidateForEmptyString_NullString_ArgumentExceptionThrown()
        {
            // Arrange
            string name = null;

            // Act & Assert
            Func<Task> action = async () => await _budgetValidation.ValidateForEmptyStringAsync(name);

            string output = $"Value cannot be null. (Parameter 'Budget Name')";

            await action.Should().ThrowAsync<ArgumentException>("Name is null").WithMessage(output);
        }

        [Fact]
        public async Task ValidateForPositiveIndex_NoExceptionThrown()
        {
            // Arrange
            int index = 1;

            // Act & Assert
            Func<Task> action = async () => await _budgetValidation.ValidateForPositiveIndexAsync(index);

            await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("Index is not negative");
        }

        [Fact]
        public async Task ValidateForPositiveIndex_ArgumentOutOfRangeExceptionThrown()
        {
            // Arrange
            int index = -99;

            // Act & Assert
            Func<Task> action = async () => await _budgetValidation.ValidateForPositiveIndexAsync(index);

            string output =
                $"{BudgetMessage.IndexMustBePositive} ('{index}') must be a non-negative value. (Parameter 'Index must be a positive number')Actual value was {index}.";
            await action.Should().ThrowAsync<ArgumentException>("Index is negative").WithMessage(output);
        }


        [Fact]
        public async Task ValidateForUserIdAsync_ValidUserId_NoExceptionThrown()
        {
            // Arrange
            int userId = 1;

            User user = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com"
            };

            _context.Users.Add(user);

            _context.SaveChanges();

            // Act & Assert
            Func<Task> action = async () => await _budgetValidation.ValidateForUserIdAsync(userId, default);

            await action.Should().NotThrowAsync<ArgumentOutOfRangeException>("UserId is Valid");
            await action.Should().NotThrowAsync<InvalidOperationException>("UserId exists");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-99)]
        [InlineData(null)]
        public async Task ValidateForUserIdAsync_NegativeUserIdOrZeroUserId_ArgumentOutOfRangeException(
            int selectedUserId)
        {
            // Arrange
            int userId = selectedUserId;

            // Act & Assert
            Func<Task> action = async () => await _budgetValidation.ValidateForUserIdAsync(userId, default);

            string output =
                $"{UserMessage.UserId} ('{userId}') must be a non-negative and non-zero value. (Parameter '{UserMessage.UserId}')Actual value was {userId}.";

            await action.Should().ThrowAsync<ArgumentOutOfRangeException>("Name is null").WithMessage(output);
        }
    }
}