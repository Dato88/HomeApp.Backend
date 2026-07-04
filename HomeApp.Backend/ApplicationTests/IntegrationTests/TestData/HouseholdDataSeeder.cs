using Bogus;
using Domain.Entities.Households;

namespace ApplicationTests.IntegrationTests.TestData;

public class HouseholdDataSeeder : BaseTest
{
    private readonly Faker<Household> _householdFaker;

    public HouseholdDataSeeder(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _householdFaker = new Faker<Household>()
            .RuleFor(u => u.Name, f => f.Lorem.Word())
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);

    public async Task<Household> GenereateDummyHousehold(params int[] personIds)
    {
        var household = _householdFaker.Generate();

        if (personIds.Length > 0)
            household.CreatedById = personIds[0];

        foreach (var personId in personIds)
            household.Members.Add(new HouseholdMember { PersonId = personId, CreatedById = personId });

        await DbContext.Households.AddAsync(household);
        await DbContext.SaveChangesAsync();

        return household;
    }
}
