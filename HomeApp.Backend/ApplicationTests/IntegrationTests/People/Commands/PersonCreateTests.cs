using ApplicationTests.IntegrationTests.Helper.CreateDummyData;

namespace ApplicationTests.IntegrationTests.People.Commands;

public class PersonCommandsCreateTests : BaseCommonPersonTest
{
    private readonly CreateDummyPeople _createDummyPeople;

    public PersonCommandsCreateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _createDummyPeople = new CreateDummyPeople(unitTestingApiFactory);

    [Fact]
    public async Task CreateAsync_AddsPersonToContext()
    {
        // Arrange
        var person = await _createDummyPeople.CreateDummyPersonModelAsync();

        // Act
        var result = await CommonPersonCommands.CreatePersonAsync(person, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value > 0);
    }

    [Fact]
    public async Task CreateAsync_ReturnsFailure_WhenPersonIsNull()
    {
        // Act
        var result = await CommonPersonCommands.CreatePersonAsync(null, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Person.CreateFailedWithMessage", result.Error.Code);
        Assert.Contains("null", result.Error.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreateAsync_CallsAllValidations_Once()
    {
        // Arrange
        var person = await _createDummyPeople.CreateDummyPersonModelAsync();

        // Act
        var result = await CommonPersonCommands.CreatePersonAsync(person, default);

        // Assert
        Assert.True(result.IsSuccess);
        PersonValidationMock.Verify(x => x.ValidateRequiredProperties(person), Times.Once);
        PersonValidationMock.Verify(x => x.ValidateMaxLength(person), Times.Once);
        PersonValidationMock.Verify(
            x => x.ValidatePersonnameDoesNotExistAsync(person.Username, It.IsAny<CancellationToken>()),
            Times.Once);
        PersonValidationMock.Verify(x => x.ValidateEmailFormat(person.Email), Times.Once);
    }
}
