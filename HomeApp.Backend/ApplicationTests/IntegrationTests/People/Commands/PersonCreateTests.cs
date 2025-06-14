﻿using ApplicationTests.IntegrationTests.TestData;
using Domain.Entities.People;
using SharedKernel;

namespace ApplicationTests.IntegrationTests.People.Commands;

public class PersonCommandsCreateTests : BaseCommonPersonTest
{
    private readonly PeopleDataSeeder _peopleDataSeeder;

    public PersonCommandsCreateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _peopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);

    [Fact]
    public async Task CreateAsync_ShouldSucceed_WhenPersonIsValid()
    {
        var person = await _peopleDataSeeder.CreatePersonModelAsync();

        // Setup validation mocks
        PersonValidationMock.Setup(x => x.ValidateRequiredProperties(person)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateMaxLength(person)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateEmailFormat(person.Email)).Returns(Result.Success());
        PersonValidationMock.Setup(x =>
                x.ValidatePersonnameDoesNotExistAsync(person.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await CommonPersonCommands.CreatePersonAsync(person, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task CreateAsync_ShouldFail_WhenPersonIsNull()
    {
        var result = await CommonPersonCommands.CreatePersonAsync(null, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Select(c => c.Code.Should().Be(PersonErrors.CreateFailedWithMessage("").Code));
    }

    [Fact]
    public async Task CreateAsync_ShouldCallAllValidationsOnce()
    {
        var person = await _peopleDataSeeder.CreatePersonModelAsync();

        // Setup validation mocks
        PersonValidationMock.Setup(x => x.ValidateRequiredProperties(person)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateMaxLength(person)).Returns(Result.Success());
        PersonValidationMock.Setup(x => x.ValidateEmailFormat(person.Email)).Returns(Result.Success());
        PersonValidationMock.Setup(x =>
                x.ValidatePersonnameDoesNotExistAsync(person.Username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var result = await CommonPersonCommands.CreatePersonAsync(person, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        PersonValidationMock.Verify(x => x.ValidateRequiredProperties(person), Times.Once);
        PersonValidationMock.Verify(x => x.ValidateMaxLength(person), Times.Once);
        PersonValidationMock.Verify(x => x.ValidateEmailFormat(person.Email), Times.Once);
        PersonValidationMock.Verify(
            x => x.ValidatePersonnameDoesNotExistAsync(person.Username, It.IsAny<CancellationToken>()), Times.Once);
    }
}
