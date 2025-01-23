namespace HomeApp.Library.Tests.Crud.PersonCrudTests;

public class PersonDeleteTests : BasePersonTest
{
    public PersonDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

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

        DbContext.People.Add(person);
        await DbContext.SaveChangesAsync();

        // Act
        await _personCrud.DeleteAsync(person.Id, default);

        // Assert
        var deletedUser = await DbContext.People.FindAsync(person.Id);
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
