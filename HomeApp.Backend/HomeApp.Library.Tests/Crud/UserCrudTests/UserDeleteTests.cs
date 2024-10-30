namespace HomeApp.Library.Tests.Crud.UserCrudTests
{
    public class UserCrudTests : BaseUserTest
    {
        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
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
            await _userCrud.DeleteAsync(user.Id, default);

            // Assert
            User? deletedUser = await _context.Users.FindAsync(user.Id);
            deletedUser.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            int nonExistingUserId = 999;

            // Act
            Func<Task> action = async () => await _userCrud.DeleteAsync(nonExistingUserId, default);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage(UserMessage.UserNotFound);
        }
    }
}