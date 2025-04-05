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
        var result = await CommonPersonCommands.UpdatePersonAsync(updatedPerson, default);

        // Assert
        Assert.True(result.IsSuccess);

        var updated = await DbContext.People.FindAsync(updatedPerson.Id);

        updated!.Id.Should().Be(updatedPerson.Id);
        updated.Username.Should().Be(updatedPerson.Username);
        updated.FirstName.Should().Be(updatedPerson.FirstName);
        updated.LastName.Should().Be(updatedPerson.LastName);
        updated.Email.Should().Be(updatedPerson.Email);
        updated.UserId.Should().NotBe(updatedPerson.UserId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCall_AllValidations()
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
        var result = await CommonPersonCommands.UpdatePersonAsync(updatedPerson, default);

        // Assert
        Assert.True(result.IsSuccess);

        PersonValidationMock.Verify(
            v => v.ValidatePersonnameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once);
        PersonValidationMock.Verify(v => v.ValidateEmailFormat(It.IsAny<string>()), Times.Once);
        PersonValidationMock.Verify(v => v.ValidateRequiredProperties(It.IsAny<Person>()), Times.Once);
        PersonValidationMock.Verify(v => v.ValidateMaxLength(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotCall_ValidatePersonnameDoesNotExistAsync_WhenUsernameIsUnchanged()
    {
        // Arrange
        var existingPerson = await _createDummyPeople.CreateDummyPersonAsync();

        var updatedPerson = new Person
        {
            Id = existingPerson.Id,
            Username = existingPerson.Username,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        // Act
        var result = await CommonPersonCommands.UpdatePersonAsync(updatedPerson, default);

        // Assert
        Assert.True(result.IsSuccess);
        PersonValidationMock.Verify(
            v => v.ValidatePersonnameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn_Failure_WhenPersonIsNull()
    {
        // Act
        var result = await CommonPersonCommands.UpdatePersonAsync(null, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Person.UpdateFailedWithMessage", result.Error.Code);
        Assert.Contains("null", result.Error.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturn_Failure_WhenPersonNotFound()
    {
        // Arrange
        var nonExistingPerson = new Person
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
        Assert.True(result.IsFailure);
        Assert.Equal("Person.NotFound", result.Error.Code);
    }
}
