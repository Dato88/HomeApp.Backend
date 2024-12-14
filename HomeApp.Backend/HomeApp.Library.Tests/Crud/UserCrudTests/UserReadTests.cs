namespace HomeApp.Library.Tests.Crud.UserCrudTests;

public class UserReadTests : BaseUserTest
{
    [Fact]
    public async Task FindByIdAsync_ReturnsUser_WhenExists()
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

        _context.Users.Add(person);
        await _context.SaveChangesAsync();

        // Act
        var result = await PersonCrud.FindByIdAsync(person.Id, default);

        // Assert
        result.Should().Be(person);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsException_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await PersonCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(UserMessage.UserNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBudgetRows()
    {
        // Arrange
        Person user1 = new()
        {
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Password = "password",
            Email = "test@example.com"
        };

        Person user2 = new()
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
        var result = await PersonCrud.GetAllAsync(default);

        // Assert
        result.Should().Contain(new[] { user1, user2 });
    }
}
