using HomeApp.Library.Models.Data_Transfer_Objects.PersonDtos;

namespace HomeApp.Library.Tests.Crud.PersonCrudTests;

public class PersonReadTests : BasePersonTest
{
    public PersonReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

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
        var result = await _personCrud.FindByIdAsync(person.Id, default);

        // Assert
        result.Should().BeEquivalentTo((PersonDto)person);
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsException_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await _personCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(PersonMessage.PersonNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBudgetRows()
    {
        // Arrange
        Person person1 = new()
        {
            Username = "testuser",
            FirstName = "John",
            LastName = "Doe",
            Email = "test@example.com",
            UserId = "safdf-adfdf-dfdsx-vcere-fooOO-1232?"
        };

        Person person2 = new()
        {
            Username = "testuser2",
            FirstName = "John2",
            LastName = "Doe2",
            Email = "test@example2.com",
            UserId = "safdf-adfdf-dfdsx-Tcere-fooOO-1232?"
        };

        DbContext.People.Add(person1);
        DbContext.People.Add(person2);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await _personCrud.GetAllAsync(default);

        // Assert
        result.Should().ContainEquivalentOf((PersonDto)person1);
        result.Should().ContainEquivalentOf((PersonDto)person2);
    }
}
