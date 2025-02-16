using HomeApp.Library.Tests.Helper.CreateDummyData;

namespace HomeApp.Library.Tests.Common.People.Commands;

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
        await CommonPersonCommands.DeletePersonAsync(person.Id, default);

        // Assert
        var deletedUser = await DbContext.People.FindAsync(person.Id);
        deletedUser.Should().BeNull();
    }
}
