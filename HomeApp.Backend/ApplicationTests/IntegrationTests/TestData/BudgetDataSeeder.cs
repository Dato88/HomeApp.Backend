using Bogus;
using Domain.Entities.Budgets;

namespace ApplicationTests.IntegrationTests.TestData;

public class BudgetDataSeeder : BaseTest
{
    private readonly PeopleDataSeeder _peopleDataSeeder;

    private readonly Faker<Budget> _budgetFaker;

    public BudgetDataSeeder(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _peopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);

        _budgetFaker = new Faker<Budget>()
            .RuleFor(u => u.Year, f => f.Date.Recent().Year)
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);
    }

    public async Task<Budget> GenereateDummyTodo(int? personId = null)
    {
        await Task.Delay(0);

        var budget = _budgetFaker.Generate();

        if (personId.HasValue) budget.PersonId = personId.Value;

        return budget;
    }
}
