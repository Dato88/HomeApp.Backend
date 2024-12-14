namespace HomeApp.Library.Tests.Crud.UserCrudTests;

public class UserUpdateTests : BaseUserTest
{
    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenUserExists()
    {
        // Arrange
        Person existingPerson = new()
        {
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Password = "password",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(existingPerson);
        await _context.SaveChangesAsync();

        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Password = "newpassword",
            Email = "updated@example.com",
            LastLogin = DateTime.UtcNow
        };

        // Act
        await PersonCrud.UpdateAsync(updatedPerson, default);

        // Assert
        existingPerson.Id.Should().Be(1);
        existingPerson.Username.Should().Be(updatedPerson.Username);
        existingPerson.FirstName.Should().Be(updatedPerson.FirstName);
        existingPerson.LastName.Should().Be(updatedPerson.LastName);
        existingPerson.Password.Should().Be(updatedPerson.Password);
        existingPerson.Email.Should().Be(updatedPerson.Email);
        existingPerson.LastLogin.Should().Be(updatedPerson.LastLogin);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCall_AllValidations()
    {
        // Arrange
        Person existingPerson = new()
        {
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Password = "password",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(existingPerson);
        await _context.SaveChangesAsync();

        // Act
        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Password = "newpassword",
            Email = "updated@example.com",
            LastLogin = DateTime.UtcNow
        };

        await PersonCrud.UpdateAsync(updatedPerson, default);

        // Assert
        var result = await _context.Users.FindAsync(existingPerson.Id);

        _userValidationMock.Verify(
            v => v.ValidateUsernameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _userValidationMock.Verify(v => v.ValidateEmailFormat(It.IsAny<string>()), Times.Once);
        _userValidationMock.Verify(v => v.ValidateRequiredProperties(It.IsAny<Person>()), Times.Once);
        _userValidationMock.Verify(v => v.ValidatePasswordStrength(It.IsAny<string>()), Times.Once);
        _userValidationMock.Verify(v => v.ValidateMaxLength(It.IsAny<Person>()), Times.Once);
        _userValidationMock.Verify(v => v.ValidateLastLoginDate(It.IsAny<DateTime>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotCall_ValidateUsernameDoesNotExistAsync()
    {
        // Arrange
        Person existingPerson = new()
        {
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Password = "password",
            Email = "test@example.com",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(existingPerson);
        await _context.SaveChangesAsync();

        // Act
        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = "testuser",
            FirstName = "Jane",
            LastName = "Doe",
            Password = "newpassword",
            Email = "updated@example.com",
            LastLogin = DateTime.UtcNow
        };

        await PersonCrud.UpdateAsync(updatedPerson, default);

        // Assert
        var result = await _context.Users.FindAsync(existingPerson.Id);

        _userValidationMock.Verify(
            v => v.ValidateUsernameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenUserIsNull()
    {
        // Act
        Func<Task> action = async () => await PersonCrud.UpdateAsync(null, default);

        // Assert
        await action.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        Person nonExistingPerson = new()
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
        Func<Task> action = async () => await PersonCrud.UpdateAsync(nonExistingPerson, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(UserMessage.UserNotFound);
    }
}
