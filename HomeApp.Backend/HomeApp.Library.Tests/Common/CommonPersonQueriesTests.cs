using HomeApp.Library.People.Dtos;

namespace HomeApp.Library.Tests.Common;

public class CommonCommonPersonQueriesTests : BasePersonFacadeTest
{
    public CommonCommonPersonQueriesTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

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
        var result = await CommonPersonCommands.CreatePersonAsync(person, default);

        // Assert
        result.Should().BeGreaterThan(0);
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
        var result = await CommonPersonQueries.GetPersonByEmailAsync(email, default);

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
        var result = await CommonPersonQueries.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletePersonAsync_RemovesPersonSuccessfully()
    {
        // Arrange
        var personId = 1;

        // Act
        await CommonPersonCommands.DeletePersonAsync(personId, default);

        // Assert
        // PersonCommandsMock.Verify(x => x.DeleteAsync(personId, It.IsAny<CancellationToken>()), Times.Once);
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
        var result = await CommonPersonCommands.UpdatePersonAsync(person, default);

        // Assert
        result.Should().Be(true);
    }
}
