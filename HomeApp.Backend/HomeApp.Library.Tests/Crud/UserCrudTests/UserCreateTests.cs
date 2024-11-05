namespace HomeApp.Library.Tests.Crud.UserCrudTests
{
    public class UserCreateTests : BaseUserTest
    {
        [Fact]
        public async Task CreateAsync_AddsUserToContext()
        {
            // Arrange
            Person person = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com"
            };

            // Act
            await PersonCrud.CreateAsync(person, default);

            // Assert
            person.Id.Should().NotBe(0);
            Assert.Contains(person, _context.Users);
        }

        [Fact]
        public async Task CreateAsync_ShouldCall_AllValidations()
        {
            // Arrange
            Person person = new()
            {
                Username = "testuser",
                FirstName = "John",
                LastName = "Doe",
                Password = "password",
                Email = "test@example.com"
            };

            // Act
            await PersonCrud.CreateAsync(person, default);

            _userValidationMock.Verify(
                x => x.ValidateUsernameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Once);
            _userValidationMock.Verify(x => x.ValidateEmailFormat(It.IsAny<string>()), Times.Once);
            _userValidationMock.Verify(x => x.ValidateRequiredProperties(It.IsAny<Person>()), Times.Once);
            _userValidationMock.Verify(x => x.ValidatePasswordStrength(It.IsAny<string>()), Times.Once);
            _userValidationMock.Verify(x => x.ValidateMaxLength(It.IsAny<Person>()), Times.Once);
            _userValidationMock.Verify(x => x.ValidateLastLoginDate(It.IsAny<DateTime?>()), Times.Once);

            // Assert
            Assert.Equal(1, _context.Users.Count());
        }

        [Fact]
        public async Task CreateAsync_ThrowsException_WhenUserIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
                await PersonCrud.CreateAsync(null, default));
        }
    }
}