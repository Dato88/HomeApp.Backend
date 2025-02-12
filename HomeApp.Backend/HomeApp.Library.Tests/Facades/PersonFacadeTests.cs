using HomeApp.Library.People.Dtos;

namespace HomeApp.Library.Tests.Facades;

public class PersonFacadeTests : BasePersonFacadeTest
{
    public PersonFacadeTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task CreatePersonAsync_CreatesPersonSuccessfully()
    {
        // Arrange
        var person = new Person
        {
            Username = "john.doe",
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            UserId = Guid.NewGuid().ToString()
        };

        // Act
        await PersonFacade.CreatePersonAsync(person, default);

        // Assert
        PersonCommandsMock.Verify(x => x.CreateAsync(person, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetPersonByEmailAsync_ReturnsPerson_WhenPersonExists()
    {
        // Arrange
        var email = "john.doe@example.com";
        var person = new Person
        {
            Email = email, FirstName = "John", LastName = "Doe", UserId = Guid.NewGuid().ToString()
        };

        DbContext.People.Add(person);
        DbContext.SaveChanges();
        PersonDto personDto = person;

        // Act
        var result = await PersonFacade.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(personDto);
    }

    [Fact]
    public async Task GetPersonByEmailAsync_ReturnsNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        // Act
        var result = await PersonFacade.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletePersonAsync_RemovesPersonSuccessfully()
    {
        // Arrange
        var personId = 1;

        // Act
        await PersonFacade.DeletePersonAsync(personId, default);

        // Assert
        PersonCommandsMock.Verify(x => x.DeleteAsync(personId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdatePersonAsync_UpdatesPersonWithNewDetails()
    {
        // Arrange
        var person = new Person
        {
            Id = 1,
            Username = "jane.doe",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            UserId = Guid.NewGuid().ToString()
        };

        // Act
        await PersonFacade.UpdatePersonAsync(person, default);

        // Assert
        PersonCommandsMock.Verify(x => x.UpdateAsync(person, It.IsAny<CancellationToken>()), Times.Once);
    }
}
