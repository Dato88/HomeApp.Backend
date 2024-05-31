namespace HomeApp.Library.Tests.Crud.UserCrudTests
{
    public class UserUpdateTests : BaseUserTest
    {
        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            User existingUser = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            User updatedUser = new()
            {
                Id = existingUser.Id,
                Username = "updateduser",
                FirstName = "Jane",
                LastName = "Doe",
                Password = "newpassword",
                Email = "updated@example.com",
                LastLogin = DateTime.UtcNow
            };

            // Act
            await _userCrud.UpdateAsync(updatedUser);

            // Assert
            existingUser.Id.Should().Be(1);
            existingUser.Username.Should().Be(updatedUser.Username);
            existingUser.FirstName.Should().Be(updatedUser.FirstName);
            existingUser.LastName.Should().Be(updatedUser.LastName);
            existingUser.Password.Should().Be(updatedUser.Password);
            existingUser.Email.Should().Be(updatedUser.Email);
            existingUser.LastLogin.Should().Be(updatedUser.LastLogin);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCall_AllValidations()
        {
            // Arrange
            User existingUser = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            // Act
            User updatedUser = new()
            {
                Id = existingUser.Id,
                Username = "updateduser",
                FirstName = "Jane",
                LastName = "Doe",
                Password = "newpassword",
                Email = "updated@example.com",
                LastLogin = DateTime.UtcNow
            };

            await _userCrud.UpdateAsync(updatedUser);

            // Assert
            User? result = await _context.Users.FindAsync(existingUser.Id);

            _userValidationMock.Verify(v => v.ValidateUsernameDoesNotExistAsync(It.IsAny<string>()), Times.Once);
            _userValidationMock.Verify(v => v.ValidateEmailFormat(It.IsAny<string>()), Times.Once);
            _userValidationMock.Verify(v => v.ValidateRequiredProperties(It.IsAny<User>()), Times.Once);
            _userValidationMock.Verify(v => v.ValidatePasswordStrength(It.IsAny<string>()), Times.Once);
            _userValidationMock.Verify(v => v.ValidateMaxLength(It.IsAny<User>()), Times.Once);
            _userValidationMock.Verify(v => v.ValidateLastLoginDate(It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldNotCall_ValidateUsernameDoesNotExistAsync()
        {
            // Arrange
            User existingUser = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();

            // Act
            User updatedUser = new()
            {
                Id = existingUser.Id,
                Username = "testuser",
                FirstName = "Jane",
                LastName = "Doe",
                Password = "newpassword",
                Email = "updated@example.com",
                LastLogin = DateTime.UtcNow
            };

            await _userCrud.UpdateAsync(updatedUser);

            // Assert
            User? result = await _context.Users.FindAsync(existingUser.Id);

            _userValidationMock.Verify(v => v.ValidateUsernameDoesNotExistAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserIsNull()
        {
            // Act
            Func<Task> action = async () => await _userCrud.UpdateAsync(null);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            User nonExistingUser = new()
            {
                Id = 999,
                Username = "nonexistinguser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "nonexisting@example.com",
                LastLogin = DateTime.UtcNow
            };

            // Act
            Func<Task> action = async () => await _userCrud.UpdateAsync(nonExistingUser);

            // Assert
            await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(UserMessage.UserNotFound);
        }
    }
}
