namespace HomeApp.Library.Tests.Crud.PersonCrudTests;

public class PersonDeleteTests : BasePersonTest
{
    [Fact]
    public async Task DeleteAsync_ShouldDeletePerson_WhenPersonExists()
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
        await _personCrud.DeleteAsync(person.Id, default);

        // Assert
        var deletedUser = await _context.People.FindAsync(person.Id);
        deletedUser.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenPersonDoesNotExist()
    {
        // Arrange
        var nonExistingUserId = 999;

        // Act
        var action = async () => await _personCrud.DeleteAsync(nonExistingUserId, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(PersonMessage.PersonNotFound);
    }
}
