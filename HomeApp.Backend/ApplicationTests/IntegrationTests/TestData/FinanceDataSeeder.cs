using Bogus;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;

namespace ApplicationTests.IntegrationTests.TestData;

public class FinanceDataSeeder : BaseTest
{
    private readonly Faker<Account> _accountFaker;
    private readonly Faker<Category> _categoryFaker;

    public FinanceDataSeeder(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _accountFaker = new Faker<Account>()
            .RuleFor(u => u.Name, f => f.Finance.AccountName())
            .RuleFor(u => u.AccountType, f => f.PickRandom<AccountType>())
            .RuleFor(u => u.CurrencyCode, _ => "EUR")
            .RuleFor(u => u.IsActive, _ => true)
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);

        _categoryFaker = new Faker<Category>()
            .RuleFor(u => u.Name, f => $"{f.Lorem.Word()}-{f.Random.AlphaNumeric(6)}")
            .RuleFor(u => u.CreatedAt, f => f.Date.RecentOffset(10).UtcDateTime);
    }

    public async Task<Account> GenereateDummyAccount(int personId, string? iban = null,
        params int[] sharedHouseholdIds)
    {
        var account = _accountFaker.Generate();
        account.PersonId = personId;
        account.CreatedById = personId;
        account.Iban = iban;

        foreach (var householdId in sharedHouseholdIds)
            account.AccountHouseholds.Add(new AccountHousehold
            {
                HouseholdId = householdId, CreatedById = personId
            });

        await DbContext.Accounts.AddAsync(account);
        await DbContext.SaveChangesAsync();

        return account;
    }

    public async Task<Category> GenereateDummyCategory(int householdId, int personId,
        CategoryType categoryType = CategoryType.Expense)
    {
        var category = _categoryFaker.Generate();
        category.HouseholdId = householdId;
        category.CategoryType = categoryType;
        category.CreatedById = personId;

        await DbContext.Categories.AddAsync(category);
        await DbContext.SaveChangesAsync();

        return category;
    }

    public async Task<Transaction> GenereateDummyTransaction(int accountId, int personId, DateOnly bookingDate,
        decimal amount, int? categoryId = null)
    {
        var transaction = new Transaction
        {
            AccountId = accountId,
            BookingDate = bookingDate,
            Amount = amount,
            CategoryId = categoryId,
            Source = TransactionSource.Manual,
            CreatedById = personId
        };

        await DbContext.Transactions.AddAsync(transaction);
        await DbContext.SaveChangesAsync();

        return transaction;
    }
}
