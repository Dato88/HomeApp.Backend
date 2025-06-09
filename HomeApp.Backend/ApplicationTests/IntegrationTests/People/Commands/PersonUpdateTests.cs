using ApplicationTests.IntegrationTests.TestData;
using Domain.Entities.People;
using SharedKernel;

namespace ApplicationTests.IntegrationTests.People.Commands;

public class PersonCommandsUpdateTests : BaseCommonPersonTest
{
    private readonly PeopleDataSeeder _peopleDataSeeder;

    public PersonCommandsUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _peopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);

    [Fact]
    public async Task UpdateAsync_ShouldSucceed_WhenPersonExists()
    {
        var existingPerson = await _peopleDataSeeder.SeedPersonAsync();

        var updatedPerson = new Person
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated@example.com",
            UserId = "new-user-id"
        };

        // Setup validation mocks
        PersonValidationMock.Setup(x => x.ValidateRequiredProperties(updatedPerson)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateMaxLength(updatedPerson)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateEmailFormat(updatedPerson.Email)).Returns(Result.Success());
        PersonValidationMock.Setup(x =>
                x.ValidatePersonnameDoesNotExistAsync(updatedPerson.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await CommonPersonCommands.UpdatePersonAsync(updatedPerson, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var updated = await DbContext.People.FindAsync(updatedPerson.Id);
        updated.Should().NotBeNull();
        updated!.Username.Should().Be(updatedPerson.Username);
        updated.FirstName.Should().Be(updatedPerson.FirstName);
        updated.LastName.Should().Be(updatedPerson.LastName);
        updated.Email.Should().Be(updatedPerson.Email);
        updated.UserId.Should().NotBe(updatedPerson.UserId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldCallAllValidations()
    {
        var existingPerson = await _peopleDataSeeder.SeedPersonAsync();

        var updated = new Person
        {
            Id = existingPerson.Id,
            Username = "updateduser",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated212@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        PersonValidationMock.Setup(x => x.ValidateRequiredProperties(updated)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateMaxLength(updated)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateEmailFormat(updated.Email)).Returns(Result.Success());
        PersonValidationMock.Setup(x =>
                x.ValidatePersonnameDoesNotExistAsync(updated.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await CommonPersonCommands.UpdatePersonAsync(updated, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        PersonValidationMock.Verify(x => x.ValidateRequiredProperties(updated), Times.Once);
        PersonValidationMock.Verify(x => x.ValidateMaxLength(updated), Times.Once);
        PersonValidationMock.Verify(x => x.ValidateEmailFormat(updated.Email), Times.Once);
        PersonValidationMock.Verify(
            x => x.ValidatePersonnameDoesNotExistAsync(updated.Username, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotCall_ValidatePersonnameDoesNotExistAsync_WhenUsernameUnchanged()
    {
        var existingPerson = await _peopleDataSeeder.SeedPersonAsync();

        var updated = new Person
        {
            Id = existingPerson.Id,
            Username = existingPerson.Username,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "updated3@example.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        PersonValidationMock.Setup(x => x.ValidateRequiredProperties(updated)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateMaxLength(updated)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateEmailFormat(updated.Email)).Returns(Result.Success());
        PersonValidationMock.Setup(x =>
                x.ValidatePersonnameDoesNotExistAsync(updated.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await CommonPersonCommands.UpdatePersonAsync(updated, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        PersonValidationMock.Verify(
            x => x.ValidatePersonnameDoesNotExistAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ShouldFail_WhenPersonIsNull()
    {
        var result = await CommonPersonCommands.UpdatePersonAsync(null, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Select(c => c.Should().Be(PersonErrors.UpdateFailedWithMessage("").Code));
    }

    [Fact]
    public async Task UpdateAsync_ShouldFail_WhenPersonNotFound()
    {
        var nonExisting = new Person
        {
            Id = 9999,
            Username = "nonexistent",
            FirstName = "Ghost",
            LastName = "User",
            Email = "ghost@example.com",
            UserId = "ghost-id"
        };

        PersonValidationMock.Setup(x => x.ValidateRequiredProperties(nonExisting)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateMaxLength(nonExisting)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateEmailFormat(nonExisting.Email)).Returns(Result.Success());

        var result = await CommonPersonCommands.UpdatePersonAsync(nonExisting, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Select(c => c.Should().Be(PersonErrors.NotFoundById(nonExisting.Id).Code));
    }
}
