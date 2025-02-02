using HomeApp.DataAccess.Models.Data_Transfer_Objects.PersonDtos;

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
        var personDto = new PersonDto(1, email, "John", "Doe", email);

        PersonQueriesMock.Setup(x =>
                x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()))
            .ReturnsAsync(personDto);

        // Act
        var result = await PersonFacade.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(email);
        PersonQueriesMock.Verify(
            x => x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()), Times.Once);
    }

    [Fact]
    public async Task GetPersonByEmailAsync_ReturnsNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        PersonQueriesMock.Setup(x =>
                x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()))
            .ReturnsAsync((PersonDto)null);

        // Act
        var result = await PersonFacade.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().BeNull();
        PersonQueriesMock.Verify(
            x => x.FindByEmailAsync(email, It.IsAny<CancellationToken>(), true, It.IsAny<string[]>()), Times.Once);
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
