using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Tests.Helper;
using Moq;
using Xunit;

namespace HomeApp.DataAccess.Tests.Cruds.People.Commands;

public class PersonCommandsCreateTests : BasePersonCommandsTest
{
    public PersonCommandsCreateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task CreateAsync_AddsPersonToContext()
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

        // Act
        await PersonCommands.CreateAsync(person, default);

        // Assert
        Assert.Contains(person, DbContext.People);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenPersonIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await PersonCommands.CreateAsync(null, default));

    [Fact]
    public async Task CreateAsync_CallsAllValidations_Once()
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

        // Act
        await PersonCommands.CreateAsync(person, default);

        // Assert
        PersonValidationMock.Verify(x => x.ValidateRequiredProperties(person), Times.Once);
        PersonValidationMock.Verify(x => x.ValidateMaxLength(person), Times.Once);
        PersonValidationMock.Verify(
            x => x.ValidatePersonnameDoesNotExistAsync(person.Username, It.IsAny<CancellationToken>()),
            Times.Once);
        PersonValidationMock.Verify(x => x.ValidateEmailFormat(person.Email), Times.Once);
    }
}
