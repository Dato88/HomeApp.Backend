namespace HomeApp.Library.Tests.Crud.UserCrudTests;

public class PersonCrudTests : BaseUserTest
{
    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
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
        await PersonCrud.DeleteAsync(person.Id, default);

        // Assert
        var deletedUser = await _context.People.FindAsync(person.Id);
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var nonExistingUserId = 999;

        // Act
        var action = async () => await PersonCrud.DeleteAsync(nonExistingUserId, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(UserMessage.UserNotFound);
    }
}
