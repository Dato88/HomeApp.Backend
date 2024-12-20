namespace HomeApp.Library.Tests.Crud.PersonCrudTests;

public class PersonCreateTests : BasePersonTest
{
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
        await _personCrud.CreateAsync(person, default);

        // Assert
        Assert.Contains(person, _context.People);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenPersonIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _personCrud.CreateAsync(null, default));

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
        await _personCrud.CreateAsync(person, default);

        // Assert
        _personValidationMock.Verify(x => x.ValidateRequiredProperties(person), Times.Once);
        _personValidationMock.Verify(x => x.ValidateMaxLength(person), Times.Once);
        _personValidationMock.Verify(
            x => x.ValidatePersonnameDoesNotExistAsync(person.Username, It.IsAny<CancellationToken>()),
            Times.Once);
        _personValidationMock.Verify(x => x.ValidateEmailFormat(person.Email), Times.Once);
    }
}
