using Bogus;
using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;

namespace ApplicationTests.IntegrationTests.TestData;

public class BudgetDataSeeder : BaseTest
{
    private readonly PeopleDataSeeder _peopleDataSeeder;

    private readonly Faker<Budget> _budgetFaker;
    private readonly Faker<BudgetGroup> _budgetGroupFaker;

    public BudgetDataSeeder(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _peopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);

        _budgetFaker = new Faker<Budget>()
            .RuleFor(u => u.Year, f => f.Date.Recent().Year)
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);

        _budgetGroupFaker = new Faker<BudgetGroup>()
            .RuleFor(u => u.Name, f => f.Lorem.Word())
            .RuleFor(u => u.BudgetGroupType, f => f.PickRandom<BudgetGroupType>())
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

    public async Task<Budget> GenereateDummyBudgetGroups(int budgetGroupCount, int? personId = null,
        bool saveAsync = true)
    {
        var budget = await GenereateDummyBudget(personId, saveAsync);

        for (var i = 0; i < budgetGroupCount; i++)
        {
            var newBudgetGroup = _budgetGroupFaker.Generate();
            newBudgetGroup.BudgetId = budget.BudgetId;
            newBudgetGroup.Index = i;
            newBudgetGroup.CreatedById = budget.CreatedById;

            await DbContext.BudgetGroups.AddAsync(newBudgetGroup);
            await DbContext.SaveChangesAsync();
        }

        return budget;
    }
}
