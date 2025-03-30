using ApplicationTests.IntegrationTests.Helper.CreateDummyData;
using Domain.Entities.People;

namespace ApplicationTests.IntegrationTests.People.Commands;

public class PersonCommandsUpdateTests : BaseCommonPersonTest
{
    private readonly CreateDummyPeople _createDummyPeople;

    public PersonCommandsUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _createDummyPeople = new CreateDummyPeople(unitTestingApiFactory);

    [Fact]
    public async Task UpdateAsync_ShouldUpdatePerson_WhenPersonExists()
    {
        // Arrange
        var existingPerson = await _createDummyPeople.CreateDummyPersonAsync();

        var updatedPerson = new Person
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        // Act
        await CommonPersonCommands.UpdatePersonAsync(updatedPerson, default);

        var result = DbContext.People.Find(updatedPerson.Id);

        // Assert
        result.Id.Should().Be(updatedPerson.Id);
        result.Username.Should().Be(updatedPerson.Username);
        result.FirstName.Should().Be(updatedPerson.FirstName);
        result.LastName.Should().Be(updatedPerson.LastName);
        result.Email.Should().Be(updatedPerson.Email);
        result.UserId.Should().NotBe(updatedPerson.UserId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCall_AllValidations()
    {
        // Arrange
        var existingPerson = await _createDummyPeople.CreateDummyPersonAsync();

        // Act
        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        await CommonPersonCommands.UpdatePersonAsync(updatedPerson, default);

        // Assert
        PersonValidationMock.Verify(
            v => v.ValidatePersonnameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        PersonValidationMock.Verify(v => v.ValidateEmailFormat(It.IsAny<string>()), Times.Once);
        PersonValidationMock.Verify(v => v.ValidateRequiredProperties(It.IsAny<Person>()), Times.Once);
        PersonValidationMock.Verify(v => v.ValidateMaxLength(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotCall_ValidatePersonnameDoesNotExistAsync()
    {
        // Arrange
        var existingPerson = await _createDummyPeople.CreateDummyPersonAsync();

        // Act
        Person updatedPerson = new()
        {
            Id = existingPerson.Id,
            Username = existingPerson.Username,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        await CommonPersonCommands.UpdatePersonAsync(updatedPerson, default);

        // Assert
        var result = await DbContext.People.FindAsync(existingPerson.Id);

        PersonValidationMock.Verify(
            v => v.ValidatePersonnameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn_False_WhenPersonIsNull()
    {
        // Act
        var result = await CommonPersonCommands.UpdatePersonAsync(null, default);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn_False_WhenPersonNotFound()
    {
        // Arrange
        Person nonExistingPerson = new()
        {
            Id = 999,
            Username = "nonexistinguser",
            FirstName = "John",
            LastName = "Doe",
            Email = "nonexisting@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        // Act
        var result = await CommonPersonCommands.UpdatePersonAsync(nonExistingPerson, default);

        // Assert
        result.Should().BeFalse();
    }
}
