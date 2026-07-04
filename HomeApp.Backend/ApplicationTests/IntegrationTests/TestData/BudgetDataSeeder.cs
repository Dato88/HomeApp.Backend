using Bogus;
using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;

namespace ApplicationTests.IntegrationTests.TestData;

public class BudgetDataSeeder : BaseTest
{
    private readonly HouseholdDataSeeder _householdDataSeeder;

    private readonly Faker<Budget> _budgetFaker;
    private readonly Faker<BudgetGroup> _budgetGroupFaker;
    private readonly Faker<BudgetRow> _budgetRowFaker;

    public BudgetDataSeeder(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _householdDataSeeder = new HouseholdDataSeeder(unitTestingApiFactory);

        _budgetFaker = new Faker<Budget>()
            .RuleFor(u => u.Year, f => f.Date.Recent().Year)
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);

        _budgetGroupFaker = new Faker<BudgetGroup>()
            .RuleFor(u => u.Title, f => f.Lorem.Word())
            .RuleFor(u => u.BudgetGroupType, f => f.PickRandom<BudgetGroupType>())
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);

        _budgetRowFaker = new Faker<BudgetRow>()
            .RuleFor(u => u.Title, f => f.Lorem.Word())
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);
    }

    public async Task<Budget> GenereateDummyBudget(int? personId = null, bool saveAsync = true)
    {
        var budget = _budgetFaker.Generate();

        if (personId.HasValue)
        {
            var household = await _householdDataSeeder.GenereateDummyHousehold(personId.Value);
            budget.HouseholdId = household.HouseholdId;
            budget.CreatedById = personId.Value;
        }

        if (saveAsync)
        {
            await DbContext.Budgets.AddAsync(budget);
            await DbContext.SaveChangesAsync();
        }

        return budget;
    }

    public async Task<Budget> GenereateDummyBudgetGroups(int budgetGroupCount, int? personId = null)
    {
        var budget = await GenereateDummyBudget(personId);

        for (var i = 0; i < budgetGroupCount; i++)
        {
            await CreateAndSaveDummyBudgetGroup(budget.BudgetId, budget.CreatedById, i);
        }

        return budget;
    }

    public async Task<Budget> GenereateDummyBudgetRows((int budgetGroup, int budgetRowCount)[] groups,
        int personId)
    {
        var budget = await GenereateDummyBudget(personId);

        for (var gi = 0; gi < groups.Length; gi++)
        {
            var budgetGroup = await CreateAndSaveDummyBudgetGroup(budget.BudgetId, personId, gi);

            for (var ri = 0; ri < groups[gi].budgetRowCount; ri++)
            {
                var newBudgetRow = _budgetRowFaker.Generate();
                newBudgetRow.BudgetGroupId = budgetGroup.BudgetGroupId;
                newBudgetRow.Index = ri;
                newBudgetRow.CreatedById = budget.CreatedById;

                await DbContext.BudgetRows.AddAsync(newBudgetRow);
                await DbContext.SaveChangesAsync();
            }
        }

        return budget;
    }

    public async Task<BudgetCell> GenereateDummyBudgetCell(int budgetRowId, int personId, int month, decimal amount)
    {
        var newBudgetCell = new BudgetCell
        {
            BudgetRowId = budgetRowId, Month = month, Amount = amount, CreatedById = personId
        };

        await DbContext.BudgetCells.AddAsync(newBudgetCell);
        await DbContext.SaveChangesAsync();

        return newBudgetCell;
    }

    private async Task<BudgetGroup> CreateAndSaveDummyBudgetGroup(int budgetId, int personId, int index)
    {
        var newBudgetGroup = _budgetGroupFaker.Generate();
        newBudgetGroup.BudgetId = budgetId;
        newBudgetGroup.Index = index;
        newBudgetGroup.CreatedById = personId;

        await DbContext.BudgetGroups.AddAsync(newBudgetGroup);
        await DbContext.SaveChangesAsync();

        return newBudgetGroup;
    }
}
