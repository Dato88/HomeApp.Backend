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
            Password = "password"
        };

        _context.Users.Add(existingPerson);
        await _context.SaveChangesAsync();

        // Act & Assert
        Func<Task> action = async () =>
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
        Action action = () => _userValidation.ValidateEmailFormat(email);
        action.Should().NotThrow<ValidationException>("Email format is valid");
    }

    [Theory]
    [InlineData("invalid.email")]
    [InlineData("invalidemail@")]
    [InlineData("invalid@.com")]
    public void ValidateEmailFormat_ShouldThrowException_WhenEmailIsInvalid(string email)
    {
        // Act & Assert
        Action action = () => _userValidation.ValidateEmailFormat(email);
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
            Password = "password",
            Email = "test@example.com"
        };

        // Act & Assert
        Action action = () => _userValidation.ValidateRequiredProperties(person);
        action.Should().NotThrow<ValidationException>("All required properties are provided");
    }

    [Theory]
    [InlineData(null, "John", "Doe", "password", "test@example.com")]
    [InlineData("testuser", null, "Doe", "password", "test@example.com")]
    [InlineData("testuser", "John", null, "password", "test@example.com")]
    [InlineData("testuser", "John", "Doe", null, "test@example.com")]
    [InlineData("testuser", "John", "Doe", "password", null)]
    public void ValidateRequiredProperties_ShouldThrowException_WhenAnyPropertyIsNull(string? username,
        string? firstName, string? lastName, string? password, string? email)
    {
        // Arrange
        Person person = new()
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            Password = password,
            Email = email
        };

        // Act & Assert
        Action action = () => _userValidation.ValidateRequiredProperties(person);
        action.Should().Throw<ValidationException>("Some required properties are missing")
            .WithMessage(UserMessage.PropertiesMissing);
    }

    [Theory]
    [InlineData("StrongPassword123!")]
    [InlineData("AnotherStrongPassword$456")]
    public void ValidatePasswordStrength_ValidPassword_ShouldNotThrowException(string password)
    {
        // Act & Assert
        _userValidation.Invoking(v => v.ValidatePasswordStrength(password)).Should()
            .NotThrow<ValidationException>();
    }

    [Theory]
    [InlineData("Weak")]
    [InlineData("Ab12!")]
    [InlineData("passwor")]
    [InlineData("p")]
    public void ValidatePasswordStrength_PasswordTooShort_ShouldThrowException(string password)
    {
        // Act & Assert
        _userValidation.Invoking(v => v.ValidatePasswordStrength(password)).Should()
            .Throw<ValidationException>("Password is too short").WithMessage(UserMessage.PasswordShort);
    }

    [Theory]
    [InlineData("onlylettErssder")]
    [InlineData("onlylettErsnouppercase")]
    public void ValidatePasswordStrength_NoDigitsOrSpecialChars_ShouldThrowException(string password)
    {
        // Act & Assert
        _userValidation.Invoking(v => v.ValidatePasswordStrength(password)).Should()
            .Throw<ValidationException>("Digit is missing").WithMessage(UserMessage.PasswordDigitMissing);
    }

    [Theory]
    [InlineData("12345678")]
    [InlineData("12345678a")]
    public void ValidatePasswordStrength_NoUpperCaseChars_ShouldThrowException(string password)
    {
        // Act & Assert
        _userValidation.Invoking(v => v.ValidatePasswordStrength(password)).Should()
            .Throw<ValidationException>("At least one Uppercase is missing")
            .WithMessage(UserMessage.PasswordUppercaseMissing);
    }

    [Theory]
    [InlineData("AbcdefgWER1ERdfadyxop")]
    [InlineData("AbcdefgweEe8rbvnb")]
    public void ValidatePasswordStrength_NoSpecialChars_ShouldThrowException(string password)
    {
        // Act & Assert
        _userValidation.Invoking(v => v.ValidatePasswordStrength(password)).Should()
            .Throw<ValidationException>("No special character provided")
            .WithMessage(UserMessage.PasswordSpecialCharMissing);
    }

    [Theory]
    [InlineData("Abcdef1!uI")]
    [InlineData("ABCDEabFG1!#")]
    [InlineData("abcdDef1!21")]
    [InlineData("ABCDEF1!pO")]
    [InlineData("AbcdefghT12!")]
    [InlineData("Abcde23fgh!")]
    [InlineData("Abcdefgxc#h1")]
    public void ValidatePasswordStrength_ValidPasswordWithDifferentCasesAndLength_ShouldNotThrowException(
        string password)
    {
        // Act & Assert
        _userValidation.Invoking(v => v.ValidatePasswordStrength(password)).Should()
            .NotThrow<ValidationException>("Password is Valid");
    }
}
