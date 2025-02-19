using HomeApp.Library.People.Dtos;

namespace HomeApp.Library.Tests.Common.People.Queries;

public class CommonPersonQueriesTests : BaseCommonPersonTest
{
    public CommonPersonQueriesTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task GetPersonByEmailAsync_ReturnsPerson_WhenPersonExists()
    {
        // Arrange
        var email = "john.doe@example.com";
        var person = new Person
        {
            Email = email, FirstName = "John", LastName = "Doe", UserId = Guid.NewGuid().ToString()
        };

        DbContext.People.Add(person);
        DbContext.SaveChanges();
        PersonDto personDto = person;

        // Act
        var result = await CommonPersonQueries.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(personDto);
    }

    [Fact]
    public async Task GetPersonByEmailAsync_ReturnsNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        // Act
        var result = await CommonPersonQueries.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().BeNull();
    }
}
