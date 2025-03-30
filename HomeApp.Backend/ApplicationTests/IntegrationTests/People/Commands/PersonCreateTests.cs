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
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync_Returns_0_WhenPersonIsNull()
    {
        // Act
        var result = await CommonPersonCommands.CreatePersonAsync(null, default);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_CallsAllValidations_Once()
    {
        // Arrange
        var person = await _createDummyPeople.CreateDummyPersonModelAsync();

        // Act
        await CommonPersonCommands.CreatePersonAsync(person, default);

        // Assert
        PersonValidationMock.Verify(x => x.ValidateRequiredProperties(person), Times.Once);
        PersonValidationMock.Verify(x => x.ValidateMaxLength(person), Times.Once);
        PersonValidationMock.Verify(
            x => x.ValidatePersonnameDoesNotExistAsync(person.Username, It.IsAny<CancellationToken>()),
            Times.Once);
        PersonValidationMock.Verify(x => x.ValidateEmailFormat(person.Email), Times.Once);
    }
}
