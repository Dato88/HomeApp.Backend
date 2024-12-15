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
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?",
            CreatedAt = DateTime.UtcNow
        };

        _context.People.Add(existingPerson);
        await _context.SaveChangesAsync();

        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        // Act
        await PersonCrud.UpdateAsync(updatedPerson, default);

        // Assert
        existingPerson.Id.Should().Be(1);
        existingPerson.Username.Should().Be(updatedPerson.Username);
        existingPerson.FirstName.Should().Be(updatedPerson.FirstName);
        existingPerson.LastName.Should().Be(updatedPerson.LastName);
        existingPerson.Email.Should().Be(updatedPerson.Email);
        existingPerson.UserId.Should().Be(updatedPerson.UserId);
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
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?",
            CreatedAt = DateTime.UtcNow
        };

        _context.People.Add(existingPerson);
        await _context.SaveChangesAsync();

        // Act
        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        await PersonCrud.UpdateAsync(updatedPerson, default);

        // Assert
        var result = await _context.People.FindAsync(existingPerson.Id);

        _userValidationMock.Verify(
            v => v.ValidateUsernameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _userValidationMock.Verify(v => v.ValidateEmailFormat(It.IsAny<string>()), Times.Once);
        _userValidationMock.Verify(v => v.ValidateRequiredProperties(It.IsAny<Person>()), Times.Once);
        _userValidationMock.Verify(v => v.ValidateMaxLength(It.IsAny<Person>()), Times.Once);
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
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?",
            CreatedAt = DateTime.UtcNow
        };

        _context.People.Add(existingPerson);
        await _context.SaveChangesAsync();

        // Act
        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = "testuser",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        await PersonCrud.UpdateAsync(updatedPerson, default);

        // Assert
        var result = await _context.People.FindAsync(existingPerson.Id);

        _userValidationMock.Verify(
            v => v.ValidateUsernameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenUserIsNull()
    {
        // Act
        var action = async () => await PersonCrud.UpdateAsync(null, default);

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
            Email = "nonexisting@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        // Act
        var action = async () => await PersonCrud.UpdateAsync(nonExistingPerson, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage(UserMessage.UserNotFound);
    }
}
