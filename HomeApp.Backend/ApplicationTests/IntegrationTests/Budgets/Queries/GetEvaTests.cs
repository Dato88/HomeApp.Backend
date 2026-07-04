using Application.Abstractions.Logging;
using Application.Features.Budgets.Queries;
using ApplicationTests.IntegrationTests.Finance;
using Domain.Entities.Budgets;
using Domain.Entities.Budgets.Enums;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Infrastructure.Features.Budgets.Queries;

namespace ApplicationTests.IntegrationTests.Budgets.Queries;

public class GetEvaTests : BaseFinanceCommandsTest
{
    public GetEvaTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    private GetEvaQueryHandler BuildHandler() => new(
        new BudgetQueries(DbContext, ExecutionContext),
        TransactionQueries,
        new Mock<IAppLogger<GetEvaQueryHandler>>().Object);

    [Fact]
    public async Task GetEva_ShouldAggregateSollAndIst()
    {
        // Arrange: household with a 2026 budget (income + expense group, rows mapped to categories)
        var personId = ExecutionContext.PersonId;
        var household = await HouseholdDataSeeder.GenereateDummyHousehold(personId);

        var salaryCategory = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId, personId,
            CategoryType.Income);
        var rentCategory = await FinanceDataSeeder.GenereateDummyCategory(household.HouseholdId, personId,
            CategoryType.Expense);

        var budget = new Budget { HouseholdId = household.HouseholdId, Year = 2026, CreatedById = personId };

        var salaryRow = new BudgetRow
        {
            Index = 0, Title = "Gehalt", CategoryId = salaryCategory.CategoryId, CreatedById = personId
        };
        var rentRow = new BudgetRow
        {
            Index = 0, Title = "Miete", CategoryId = rentCategory.CategoryId, CreatedById = personId
        };

        for (var month = 1; month <= 3; month++)
        {
            salaryRow.BudgetCells.Add(new BudgetCell { Month = month, Amount = 2500, CreatedById = personId });
            rentRow.BudgetCells.Add(new BudgetCell { Month = month, Amount = 950, CreatedById = personId });
        }

        var incomeGroup = new BudgetGroup
        {
            Index = 0, Title = "Einnahmen", BudgetGroupType = BudgetGroupType.Income, CreatedById = personId
        };
        incomeGroup.BudgetRows.Add(salaryRow);

        var expenseGroup = new BudgetGroup
        {
            Index = 1,
            Title = "Wohnen",
            BudgetGroupType = BudgetGroupType.Expense,
            TargetPercent = 30,
            CreatedById = personId
        };
        expenseGroup.BudgetRows.Add(rentRow);

        budget.BudgetGroups.Add(incomeGroup);
        budget.BudgetGroups.Add(expenseGroup);
        await DbContext.Budgets.AddAsync(budget);
        await DbContext.SaveChangesAsync();

        // Shared account with transactions: salary Jan+Feb, rent Jan, one uncategorized booking
        var account = await FinanceDataSeeder.GenereateDummyAccount(personId, null, household.HouseholdId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 28), 2500, salaryCategory.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 2, 27), 2500, salaryCategory.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 3), -950, rentCategory.CategoryId);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2026, 1, 15), -100);

        // Noise that must not leak in: a private (unshared) account and a booking from another year
        var privateAccount = await FinanceDataSeeder.GenereateDummyAccount(personId);
        await FinanceDataSeeder.GenereateDummyTransaction(privateAccount.AccountId, personId,
            new DateOnly(2026, 1, 10), -999, null);
        await FinanceDataSeeder.GenereateDummyTransaction(account.AccountId, personId,
            new DateOnly(2025, 12, 31), -888, rentCategory.CategoryId);

        // Act
        var result = await BuildHandler().Handle(new GetEvaQuery(household.HouseholdId, 2026),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var eva = result.Value;

        eva.BudgetId.Should().Be(budget.BudgetId);
        eva.Groups.Should().HaveCount(2);

        var income = eva.Groups.Single(g => g.BudgetGroupType == BudgetGroupType.Income);
        var incomeRow = income.Rows.Single();
        incomeRow.CategoryName.Should().Be(salaryCategory.Name);
        incomeRow.Soll[0].Should().Be(2500);
        incomeRow.YearSoll.Should().Be(7500);
        incomeRow.Ist[0].Should().Be(2500);
        incomeRow.Ist[1].Should().Be(2500);
        incomeRow.Ist[2].Should().Be(0);
        incomeRow.YearIst.Should().Be(5000);

        var expense = eva.Groups.Single(g => g.BudgetGroupType == BudgetGroupType.Expense);
        expense.TargetPercent.Should().Be(30);
        var expenseRow = expense.Rows.Single();
        expenseRow.Ist[0].Should().Be(950);
        expenseRow.Diff[0].Should().Be(0);
        expenseRow.Diff[1].Should().Be(950);
        expense.YearSoll.Should().Be(2850);
        expense.YearIst.Should().Be(950);
        expense.PlannedPercentOfIncome.Should().Be(38.00m);
        expense.ActualPercentOfIncome.Should().Be(19.00m);

        eva.Totals.YearIncomeSoll.Should().Be(7500);
        eva.Totals.YearIncomeIst.Should().Be(5000);
        eva.Totals.YearExpenseIst.Should().Be(950);
        eva.Totals.YearDiffIst.Should().Be(4050);
        eva.Totals.DiffIst[0].Should().Be(1550);

        eva.UnassignedIst[0].Should().Be(-100);
        eva.UnassignedTransactionCount.Should().Be(1);
    }

    [Fact]
    public async Task GetEva_ShouldReturnNotFoundForNonMember()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignHousehold = await HouseholdDataSeeder.GenereateDummyHousehold(otherPerson.PersonId);
        var budget = new Budget
        {
            HouseholdId = foreignHousehold.HouseholdId, Year = 2026, CreatedById = otherPerson.PersonId
        };
        await DbContext.Budgets.AddAsync(budget);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await BuildHandler().Handle(new GetEvaQuery(foreignHousehold.HouseholdId, 2026),
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BudgetErrors.NotFoundAll);
    }

    [Fact]
    public async Task GetMonthlyCategoryTotals_ShouldTranslateWithManyTransactions()
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
        var result = await TransactionQueries.GetMonthlyCategoryTotalsAsync(household.HouseholdId, 2026,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Sum(t => t.Count).Should().Be(1000);
        result.Value.Sum(t => t.Sum).Should().Be(-1000);
        result.Value.Where(t => t.CategoryId == category.CategoryId).Sum(t => t.Count).Should().Be(500);
    }
}
