using ApplicationTests.IntegrationTests.Helper.CreateDummyData;
using Domain.Entities.People;

namespace ApplicationTests.IntegrationTests.People.Commands;

public class PersonCommandsDeleteTests : BaseCommonPersonTest
{
    private readonly CreateDummyPeople _createDummyPeople;

    public PersonCommandsDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _createDummyPeople = new CreateDummyPeople(unitTestingApiFactory);

    [Fact]
    public async Task DeleteAsync_ShouldDeletePerson_WhenPersonExists()
    {
        // Arrange
        var person = await _createDummyPeople.CreateDummyPersonAsync();

        // Act
        var result = await CommonPersonCommands.DeletePersonAsync(person.Id, default);

        // Assert
        Assert.True(result.IsSuccess);
        var deletedUser = await DbContext.People.FindAsync(person.Id);
        deletedUser.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task DeleteAsync_ShouldReturn_Failure_WhenIdIsInvalid(int invalidId)
    {
        // Act
        var result = await CommonPersonCommands.DeletePersonAsync(invalidId, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PersonErrors.DeleteFailed(invalidId).Code, result.Error.Code);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturn_Failure_WhenPersonDoesNotExist()
    {
        // Arrange
        const int nonExistentId = 9999;

        // Act
        var result = await CommonPersonCommands.DeletePersonAsync(nonExistentId, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PersonErrors.NotFoundById(nonExistentId).Code, result.Error.Code);
        Assert.Contains(nonExistentId.ToString(), result.Error.Description);
    }
}
