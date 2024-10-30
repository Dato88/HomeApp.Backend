namespace HomeApp.Library.Tests.Crud.UserCrudTests
{
    public class UserReadTests : BaseUserTest
    {
        [Fact]
        public async Task FindByIdAsync_ReturnsUser_WhenExists()
        {
            // Arrange
            User user = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            User? result = await _userCrud.FindByIdAsync(user.Id, default);

            // Assert
            result.Should().Be(user);
        }

        [Fact]
        public async Task FindByIdAsync_ReturnsException_WhenNotExists()
        {
            // Assert
            Func<Task> action = async () => await _userCrud.FindByIdAsync(999, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(UserMessage.UserNotFound);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllBudgetRows()
        {
            // Arrange
            User user1 = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com"
            };

            User user2 = new()
            {
                Username = "testuser2",
                FirstName = "John2",
                LastName = "Doe2",
                Password = "password2",
                Email = "test@example2.com"
            };

            _context.Users.Add(user1);
            _context.Users.Add(user2);
            await _context.SaveChangesAsync();

            // Act
            IEnumerable<User>? result = await _userCrud.GetAllAsync(default);

            // Assert
            result.Should().Contain(new[] { user1, user2 });
        }
    }
}