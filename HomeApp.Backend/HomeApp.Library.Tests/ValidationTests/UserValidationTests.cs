using System.ComponentModel.DataAnnotations;
using HomeApp.Library.Validations;

namespace HomeApp.Library.Tests.ValidationTests;

public class UserValidationTests
{
    private readonly HomeAppContext _context;
    private readonly UserValidation _userValidation;

    public UserValidationTests()
    {
        _context = StaticLibraryHelper.CreateInMemoryContext();
        _userValidation = new UserValidation(_context);
    }

    [Fact]
    public async Task ValidateUsernameDoesNotExistAsync_ShouldThrowException_WhenUsernameExists()
    {
        // Arrange
        var existingUsername = "existinguser";

        Person existingPerson = new()
        {
            Username = existingUsername,
            Email = "existing@example.com",
            FirstName = "Existing",
            LastName = "User",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        _context.People.Add(existingPerson);
        await _context.SaveChangesAsync();

        // Act & Assert
        var action = async () =>
            await _userValidation.ValidateUsernameDoesNotExistAsync(existingUsername, default);

        await action.Should().ThrowAsync<InvalidOperationException>("Username already exists")
            .WithMessage(UserMessage.UserAlreadyExists);
    }

    [Theory]
    [InlineData("valid@example.com")]
    [InlineData("another_valid.email@example.com")]
    public void ValidateEmailFormat_ShouldNotThrowException_WhenEmailIsValid(string email)
    {
        // Act & Assert
        var action = () => _userValidation.ValidateEmailFormat(email);
        action.Should().NotThrow<ValidationException>("Email format is valid");
    }

    [Theory]
    [InlineData("invalid.email")]
    [InlineData("invalidemail@")]
    [InlineData("invalid@.com")]
    public void ValidateEmailFormat_ShouldThrowException_WhenEmailIsInvalid(string email)
    {
        // Act & Assert
        var action = () => _userValidation.ValidateEmailFormat(email);
        action.Should().Throw<ValidationException>("Email format is invalid").WithMessage(UserMessage.InvalidEmail);
    }

    [Fact]
    public void ValidateRequiredProperties_ShouldNotThrowException_WhenAllPropertiesAreNotEmpty()
    {
        // Arrange
        Person person = new()
        {
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        // Act & Assert
        var action = () => _userValidation.ValidateRequiredProperties(person);
        action.Should().NotThrow<ValidationException>("All required properties are provided");
    }

    [Theory]
    [InlineData("testuser", null, "Doe", "test@example.com", "safdf-adfdf-dfdsx-Tcere-fooOO-1232?")]
    [InlineData("testuser", "John", null, "test@example.com", "safdf-adfdf-dfdsx-Tcere-fooOO-1232?")]
    [InlineData("testuser", "John", "Doe", null, "safdf-adfdf-dfdsx-Tcere-fooOO-1232?")]
    [InlineData("testuser", "John", "Doe", "test@example.com", null)]
    public void ValidateRequiredProperties_ShouldThrowException_WhenAnyPropertyIsNull(string? username,
        string? firstName, string? lastName, string? email, string? userId)
    {
        // Arrange
        Person person = new()
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserId = userId
        };

        // Act & Assert
        var action = () => _userValidation.ValidateRequiredProperties(person);
        action.Should().Throw<ValidationException>("Some required properties are missing")
            .WithMessage(UserMessage.PropertiesMissing);
    }
}
