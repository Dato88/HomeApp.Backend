using Bogus;
using Person = Domain.Entities.People.Person;

namespace ApplicationTests.IntegrationTests.TestData;

public class PeopleDataSeeder : BaseTest
{
    private readonly Faker<Person> _personFaker;

    public PeopleDataSeeder(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _personFaker = new Faker<Person>()
            .RuleFor(u => u.FirstName, f => f.Name.FirstName())
            .RuleFor(u => u.LastName, f => f.Name.LastName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.UserId, f => f.Random.Guid().ToString());

    public async Task SeedAsync()
    {
        for (var i = 0; i < 10; i++) await SeedPersonAsync(false);

        await DbContext.SaveChangesAsync();
    }

    public async Task<Person> CreatePersonModelAsync()
    {
        await Task.Delay(0);

        var person = _personFaker.Generate();

        return person;
    }

    public async Task<Person> SeedPersonAsync(bool saveAsync = true)
    {
        var newPerson = await CreatePersonModelAsync();

        await DbContext.People.AddAsync(newPerson);

        if (saveAsync)
            await DbContext.SaveChangesAsync();

        return newPerson;
    }
}
