using Bogus;
using Person = HomeApp.DataAccess.Models.Person;

namespace HomeApp.Library.Tests.Helper.CreateDummyData;

public class CreateDummyPeople : BaseTest
{
    private readonly Faker<Person> _personFaker;

    public CreateDummyPeople(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        Randomizer.Seed = new Random(1337);

        _personFaker = new Faker<Person>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.UserId, f => f.Random.Guid().ToString());
    }

    public async Task<Person> CreateDummyPersonModelAsync()
    {
        await Task.Delay(0);

        var person = _personFaker.Generate();

        return person;
    }

    public async Task<Person> CreateDummyPersonAsync()
    {
        var newPerson = _personFaker.Generate();

        await DbContext.People.AddAsync(newPerson);
        await DbContext.SaveChangesAsync();

        return newPerson;
    }
}
