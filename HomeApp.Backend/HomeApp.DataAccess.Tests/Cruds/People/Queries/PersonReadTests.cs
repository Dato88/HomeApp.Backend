using HomeApp.DataAccess.Models.Data_Transfer_Objects.PersonDtos;
using HomeApp.DataAccess.Tests.Helper;

namespace HomeApp.DataAccess.Tests.Cruds.People.Queries;

public class PersonCommandsReadTests : BasePersonQueriesTest
{
    public PersonCommandsReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task FindByIdAsync_ReturnsPersonDto_WhenPersonExists()
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

        DbContext.People.Add(person);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await PersonQueries.FindByIdAsync(person.Id, default);

        // Assert
        result.Should().BeEquivalentTo((PersonDto)person);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsException_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await PersonQueries.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(PersonMessage.PersonNotFound);
    }
}
