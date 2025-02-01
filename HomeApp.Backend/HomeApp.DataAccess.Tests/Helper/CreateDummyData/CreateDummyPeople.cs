namespace HomeApp.DataAccess.Tests.Helper.CreateDummyData;

public class CreateDummyPeople : BaseTest
{
    public CreateDummyPeople(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    public async Task<Person> CreateOneDummyPerson()
    {
        var newPerson = new Person
        {
            FirstName = "TestFirstName", LastName = "TestLastName", Email = "test@test.com", UserId = "testUserId"
        };

        await DbContext.People.AddAsync(newPerson);
        await DbContext.SaveChangesAsync();

        return newPerson;
    }
}
