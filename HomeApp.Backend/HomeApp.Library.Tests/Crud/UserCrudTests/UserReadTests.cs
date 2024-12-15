using HomeApp.Library.Models.Data_Transfer_Objects.PersonDtos;

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
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-vcere-fooOO-1232?"
        };

        _context.People.Add(person);
        await _context.SaveChangesAsync();

        // Act
        var result = await PersonCrud.FindByIdAsync(person.Id, default);

        // Assert
        result.Should().BeEquivalentTo((PersonDto)person);
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
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-vcere-fooOO-1232?"
        };

        Person user2 = new()
        {
            Username = "testuser2",
            FirstName = "John2",
            LastName = "Doe2",
            Email = "test@example2.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        _context.People.Add(user1);
        _context.People.Add(user2);
        await _context.SaveChangesAsync();

        // Act
        var result = await PersonCrud.GetAllAsync(default);

        // Assert
        result.Should().ContainEquivalentOf((PersonDto)user1);
        result.Should().ContainEquivalentOf((PersonDto)user2);
    }
}
