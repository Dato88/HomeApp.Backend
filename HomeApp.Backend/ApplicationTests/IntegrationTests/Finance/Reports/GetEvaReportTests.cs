using Application.Abstractions.Logging;
using Application.Features.Finance.Reports.Queries;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;

namespace ApplicationTests.IntegrationTests.Finance.Reports;

public class GetEvaReportTests : BaseFinanceCommandsTest
{
    public GetEvaReportTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    private GetEvaReportQueryHandler BuildHandler() => new(
        ReportQueries,
        new Mock<IAppLogger<GetEvaReportQueryHandler>>().Object);

    [Fact]
    public async Task GetEvaReport_ShouldAggregateFromTransactions()
    {
        // Arrange: groups and categories only — no manual plan; everything derives from transactions
        var personId = ExecutionContext.PersonId;
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(personId);

        var incomeGroup = await FinanceDataSeeder.GenereateDummyCategoryGroup(household.HouseholdId, personId,
            CategoryType.Income);
        var expenseGroup = await FinanceDataSeeder.GenereateDummyCategoryGroup(household.HouseholdId, personId,
            CategoryType.Expense, 30);

        var salaryCategory = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId, personId,
            CategoryType.Income, incomeGroup.CategoryGroupId);
        var rentCategory = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId, personId,
            CategoryType.Expense, expenseGroup.CategoryGroupId);
        var hobbyCategory = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId, personId,
            CategoryType.Expense);

        // Shared account: salary Jan+Feb, rent Jan, ungrouped hobby Mar, one uncategorized booking
        var account = await FinanceDataSeeder.GenereateDummyAccount(personId, null, household.HouseholdId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 28), 2500, salaryCategory.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 2, 27), 2500, salaryCategory.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 3), -950, rentCategory.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 3, 15), -60, hobbyCategory.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 15), -100);

        // Noise that must not leak in: a private (unshared) account and a booking from another year
        var privateAccount = await FinanceDataSeeder.GenereateDummyAccount(personId);
        await FinanceDataSeeder.GenereateDummyTransaction(privateAccount.AccountId, personId,
            new DateOnly(2026, 1, 10), -999, null);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2025, 12, 31), -888, rentCategory.CategoryId);

        // Act
        var result = await BuildHandler().Handle(new GetEvaReportQuery([household.HouseholdId], 2026),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var eva = result.Value;

        eva.Year.Should().Be(2026);
        eva.Groups.Should().HaveCount(2);

        var income = eva.Groups.Single(g => g.CategoryGroupType == CategoryType.Income);
        var salary = income.Categories.Single(c => c.CategoryId == salaryCategory.CategoryId);
        salary.Ist[0].Should().Be(2500);
        salary.Ist[1].Should().Be(2500);
        salary.Ist[2].Should().Be(0);
        salary.YearIst.Should().Be(5000);
        income.YearIst.Should().Be(5000);

        var expense = eva.Groups.Single(g => g.CategoryGroupType == CategoryType.Expense);
        expense.TargetPercent.Should().Be(30);
        var rent = expense.Categories.Single(c => c.CategoryId == rentCategory.CategoryId);
        rent.Ist[0].Should().Be(950);
        expense.YearIst.Should().Be(950);
        expense.ActualPercentOfIncome.Should().Be(19.00m);

        var hobby = eva.UngroupedCategories.Single(c => c.CategoryId == hobbyCategory.CategoryId);
        hobby.Ist[2].Should().Be(60);
        hobby.YearIst.Should().Be(60);

        eva.Totals.YearIncomeIst.Should().Be(5000);
        eva.Totals.YearExpenseIst.Should().Be(1010);
        eva.Totals.YearDiffIst.Should().Be(3990);
        eva.Totals.DiffIst[0].Should().Be(1550);

        eva.UnassignedIst[0].Should().Be(-100);
        eva.UnassignedTransactionCount.Should().Be(1);
    }

    [Fact]
    public async Task GetEvaReport_ShouldCountSharedAccountOnceAcrossHouseholds()
    {
        // Arrange: one account shared into BOTH selected households must not be double counted
        var personId = ExecutionContext.PersonId;
        var householdA = await HouseholdDataSeeder.GenereateDummyHousehold(personId);
        var householdB = await HouseholdDataSeeder.GenereateDummyHousehold(personId);

        var category = await FinanceDataSeeder.GenereateDummyCategory(householdA.HouseholdId, personId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(personId, null,
            householdA.HouseholdId, householdB.HouseholdId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 3), -50, category.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 10), -100);

        // Act
        var result = await BuildHandler().Handle(
            new GetEvaReportQuery([householdA.HouseholdId, householdB.HouseholdId], 2026),
            CancellationToken.None);

        // Assert: both amounts appear exactly once (not once per selected household)
        result.IsSuccess.Should().BeTrue();
        var eva = result.Value;

        var categorized = eva.UngroupedCategories.Single(c => c.CategoryId == category.CategoryId);
        categorized.Ist[0].Should().Be(50);
        eva.Totals.YearExpenseIst.Should().Be(50);
        eva.UnassignedIst[0].Should().Be(-100);
        eva.UnassignedTransactionCount.Should().Be(1);
    }

    [Fact]
    public async Task GetEvaReport_ShouldBucketCategoryOfUnrequestedHouseholdAsUnassigned()
    {
        // Arrange: account shared into A and B, booking categorized with a B category,
        // but the report is requested for A only — the booking must land in the unassigned bucket
        var personId = ExecutionContext.PersonId;
        var householdA = await HouseholdDataSeeder.GenereateDummyHousehold(personId);
        var householdB = await HouseholdDataSeeder.GenereateDummyHousehold(personId);

        var categoryInB = await FinanceDataSeeder.GenereateDummyCategory(householdB.HouseholdId, personId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(personId, null,
            householdA.HouseholdId, householdB.HouseholdId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 4, 1), -75, categoryInB.CategoryId);

        // Act
        var result = await BuildHandler().Handle(new GetEvaReportQuery([householdA.HouseholdId], 2026),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var eva = result.Value;

        eva.UngroupedCategories.Should().NotContain(c => c.CategoryId == categoryInB.CategoryId);
        eva.UnassignedIst[3].Should().Be(-75);
        eva.UnassignedTransactionCount.Should().Be(1);
        eva.Totals.YearExpenseIst.Should().Be(0);
    }

    [Fact]
    public async Task GetEvaReport_ShouldReturnErrorWhenNotMemberOfEveryHousehold()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var ownHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(ExecutionContext.PersonId);
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);

        // Act
        var result = await BuildHandler().Handle(
            new GetEvaReportQuery([ownHousehold.HouseholdId, foreignHousehold.HouseholdId], 2026),
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.ReportFailedWithMessage("HouseholdId is invalid"));
    }

    [Fact]
    public async Task GetEvaReportData_ShouldTranslateWithManyTransactions()
    {
        // Arrange: guards the EF GroupBy/DateOnly translation on Npgsql with a realistic volume
        var personId = ExecutionContext.PersonId;
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(personId);
        var category = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId, personId);
        var account = await FinanceDataSeeder.GenereateDummyAccount(personId, null, household.HouseholdId);

        var transactions = new List<Transaction>();
        for (var i = 0; i < 1000; i++)
            transactions.Add(new Transaction
            {
                AccountId = account.AccountId,
                BookingDate = new DateOnly(2026, i % 12 + 1, i % 28 + 1),
                Amount = -1,
                CategoryId = i % 2 == 0 ? category.CategoryId : null,
                Source = TransactionSource.Manual,
                CreatedById = personId
            });
        await DbContext.Transactions.AddRangeAsync(transactions);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await ReportQueries.GetEvaReportDataAsync([household.HouseholdId], 2026,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Totals.Sum(t => t.Count).Should().Be(1000);
        result.Value.Totals.Sum(t => t.Sum).Should().Be(-1000);
        result.Value.Totals.Where(t => t.CategoryId == category.CategoryId).Sum(t => t.Count).Should().Be(500);
    }
}
