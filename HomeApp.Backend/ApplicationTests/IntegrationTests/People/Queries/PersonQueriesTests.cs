using Application.Features.People.Dtos;
using Domain.Entities.People;

namespace ApplicationTests.IntegrationTests.People.Queries;

public class PersonQueriesTests : BaseCommonPersonTest
{
    public PersonQueriesTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

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
        await DbContext.SaveChangesAsync();
        PersonResponse personResponse = person;

        // Act
        var result = await PersonQueries.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(personResponse);
    }

    [Fact]
    public async Task GetPersonByEmailAsync_ReturnsNull_WhenPersonDoesNotExist()
    {
        // Arrange
        var email = "nonexistent@example.com";

        // Act
        var result = await PersonQueries.GetPersonByEmailAsync(email, default);

        // Assert
        result.Should().BeNull();
    }
}
