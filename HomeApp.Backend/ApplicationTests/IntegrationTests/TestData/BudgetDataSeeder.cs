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

    public async Task<Budget> GenereateDummyBudget(int? personId = null, bool saveAsync = true)
    {
        var budget = _budgetFaker.Generate();

        if (personId.HasValue) budget.PersonId = personId.Value;

        if (saveAsync)
        {
            await DbContext.Budgets.AddAsync(budget);
            await DbContext.SaveChangesAsync();
        }

        return budget;
    }
}
