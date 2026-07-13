using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Finance;

public class PaymentPartnerTests : BaseFinanceCommandsTest
{
    public PaymentPartnerTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    private static ParsedTransaction Parsed(DateOnly bookingDate, decimal amount, string? name, string? iban,
        string? purpose = null) =>
        new(bookingDate, bookingDate, amount, "EUR", name, iban, purpose, null);

    [Fact]
    public async Task Import_ShouldCreateAndReusePartnersByIban()
    {
        // Arrange: two spellings of the same partner behind one IBAN plus a second partner
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        const string reweIban = "DE89370400440532013000";

        var items = new List<ParsedTransaction>
        {
            Parsed(new DateOnly(2026, 3, 2), -49.99m, "REWE Markt GmbH", reweIban),
            Parsed(new DateOnly(2026, 3, 9), -12.30m, "REWE  MARKT  GMBH", "de89 3704 0044 0532 0130 00"),
            Parsed(new DateOnly(2026, 3, 5), -950.00m, "Vermieter Müller", "DE02120300000000202051")
        };

        // Act
        var import = await TransactionCommands.ImportTransactionsAsync(account.AccountId, items,
            TransactionSource.CsvImport, CancellationToken.None);
        var reimport = await TransactionCommands.ImportTransactionsAsync(account.AccountId, items,
            TransactionSource.CsvImport, CancellationToken.None);

        // Assert: one partner per IBAN, both REWE bookings linked to the same partner
        import.IsSuccess.Should().BeTrue();
        import.Value.Imported.Should().Be(3);
        reimport.Value.Imported.Should().Be(0);
        reimport.Value.SkippedDuplicates.Should().Be(3);

        var partners = await DbContext.PaymentPartners.AsNoTracking()
            .Where(p => p.PersonId == ExecutionContext.PersonId)
            .ToListAsync();
        partners.Should().HaveCount(2);

        var rewe = partners.Single(p => p.Iban == reweIban);
        rewe.NormalizedName.Should().Be("REWE MARKT GMBH");

        var reweTransactions = await DbContext.Transactions.AsNoTracking()
            .Where(t => t.AccountId == account.AccountId && t.Amount != -950.00m)
            .ToListAsync();
        reweTransactions.Should().OnlyContain(t => t.PaymentPartnerId == rewe.PaymentPartnerId);
    }

    [Fact]
    public async Task Import_ShouldAttachNameOnlyRowsToExistingPartner()
    {
        // Arrange: first an IBAN partner, then a card payment with the same name but no IBAN
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        const string iban = "DE02120300000000202051";

        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [Parsed(new DateOnly(2026, 3, 5), -950.00m, "Vermieter Müller", iban)],
            TransactionSource.CsvImport, CancellationToken.None);

        // Act: name-only row (matches), same name with a DIFFERENT IBAN (own partner)
        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [
                Parsed(new DateOnly(2026, 4, 5), -950.00m, "vermieter   müller", null),
                Parsed(new DateOnly(2026, 4, 6), -100.00m, "Vermieter Müller", "DE44500105175407324931")
            ],
            TransactionSource.CsvImport, CancellationToken.None);

        // Assert
        var partners = await DbContext.PaymentPartners.AsNoTracking()
            .Where(p => p.PersonId == ExecutionContext.PersonId)
            .ToListAsync();
        partners.Should().HaveCount(2);

        var original = partners.Single(p => p.Iban == iban);
        var linkedCount = await DbContext.Transactions.AsNoTracking()
            .CountAsync(t => t.PaymentPartnerId == original.PaymentPartnerId);
        linkedCount.Should().Be(2, "the name-only row attaches to the existing IBAN partner");
    }

    [Fact]
    public async Task Import_ShouldLeavePartnerNullWithoutNameAndIban()
    {
        // Arrange: Revolut rows carry neither partner name nor IBAN
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        // Act
        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [Parsed(new DateOnly(2026, 3, 2), -9.99m, null, null, "Kartenzahlung unterwegs")],
            TransactionSource.CsvImport, CancellationToken.None);

        // Assert
        var transaction = await DbContext.Transactions.AsNoTracking()
            .SingleAsync(t => t.AccountId == account.AccountId);
        transaction.PaymentPartnerId.Should().BeNull();
        (await DbContext.PaymentPartners.CountAsync(p => p.PersonId == ExecutionContext.PersonId))
            .Should().Be(0);
    }

    [Fact]
    public async Task Import_ShouldLinkOwnAccountAsSelfTransfer()
    {
        // Arrange: the partner IBAN belongs to a second own account (self-transfer)
        var checking = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var savings = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId,
            "DE44500105175407324931");

        // Act
        await TransactionCommands.ImportTransactionsAsync(checking.AccountId,
            [Parsed(new DateOnly(2026, 3, 1), -500.00m, "Andrej Miller", "de44 5001 0517 5407 3249 31")],
            TransactionSource.CsvImport, CancellationToken.None);

        // Assert
        var partner = await DbContext.PaymentPartners.AsNoTracking()
            .SingleAsync(p => p.PersonId == ExecutionContext.PersonId);
        partner.Iban.Should().Be("DE44500105175407324931");
        partner.LinkedAccountId.Should().Be(savings.AccountId);
    }

    [Fact]
    public async Task CreateAndUpdateTransaction_ShouldResolveAndUnlinkPartner()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        // Act: manual create resolves a partner
        var createResult = await TransactionCommands.CreateTransactionAsync(new Transaction
        {
            AccountId = account.AccountId,
            BookingDate = new DateOnly(2026, 3, 5),
            Amount = -42.00m,
            PaymentPartnerName = "Nagelstudio Wüst",
            Source = TransactionSource.Manual
        }, CancellationToken.None);

        var created = await DbContext.Transactions.AsNoTracking()
            .SingleAsync(t => t.TransactionId == createResult.Value);
        created.PaymentPartnerId.Should().NotBeNull();

        // Act: clearing both partner fields unlinks the partner
        var updateResult = await TransactionCommands.UpdateTransactionAsync(new Transaction
        {
            TransactionId = createResult.Value,
            BookingDate = new DateOnly(2026, 3, 5),
            Amount = -42.00m,
            PaymentPartnerName = null,
            PaymentPartnerIban = null
        }, CancellationToken.None);

        // Assert
        updateResult.IsSuccess.Should().BeTrue();
        var updated = await DbContext.Transactions.AsNoTracking()
            .SingleAsync(t => t.TransactionId == createResult.Value);
        updated.PaymentPartnerId.Should().BeNull();
    }

    [Fact]
    public async Task Rename_ShouldChangeDisplayNameOnlyAndKeepMatching()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [Parsed(new DateOnly(2026, 3, 2), -23.45m, "REWE SAGT DANKE 4711", null)],
            TransactionSource.CsvImport, CancellationToken.None);

        var partner = await DbContext.PaymentPartners
            .SingleAsync(p => p.PersonId == ExecutionContext.PersonId);

        // Act: rename, then import another booking with the original raw name
        var rename = await PaymentPartnerCommands.RenamePaymentPartnerAsync(partner.PaymentPartnerId,
            "REWE", CancellationToken.None);

        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [Parsed(new DateOnly(2026, 4, 2), -12.34m, "REWE SAGT DANKE 4711", null)],
            TransactionSource.CsvImport, CancellationToken.None);

        // Assert: no second partner was created, display name stays renamed
        rename.IsSuccess.Should().BeTrue();
        var partners = await DbContext.PaymentPartners.AsNoTracking()
            .Where(p => p.PersonId == ExecutionContext.PersonId)
            .ToListAsync();
        partners.Should().HaveCount(1);
        partners[0].DisplayName.Should().Be("REWE");
        partners[0].NormalizedName.Should().Be("REWE SAGT DANKE 4711");

        (await DbContext.Transactions.CountAsync(t => t.PaymentPartnerId == partner.PaymentPartnerId))
            .Should().Be(2);
    }

    [Fact]
    public async Task Merge_ShouldMoveTransactionsDeleteSourceAndInheritIban()
    {
        // Arrange: name-only target, IBAN source linked to an own account
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var savings = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId,
            "DE44500105175407324931");

        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [
                Parsed(new DateOnly(2026, 3, 1), -100.00m, "Tagesgeld", null),
                Parsed(new DateOnly(2026, 3, 2), -200.00m, "Andrej Miller", "DE44500105175407324931")
            ],
            TransactionSource.CsvImport, CancellationToken.None);

        var target = await DbContext.PaymentPartners.AsNoTracking()
            .SingleAsync(p => p.PersonId == ExecutionContext.PersonId && p.Iban == null);
        var source = await DbContext.PaymentPartners.AsNoTracking()
            .SingleAsync(p => p.PersonId == ExecutionContext.PersonId && p.Iban != null);

        // The merge runs in a fresh request scope in production; without this the transactions
        // tracked by the import above would be fixed up to null when the source partner is removed
        DbContext.ChangeTracker.Clear();

        // Act
        var merge = await PaymentPartnerCommands.MergePaymentPartnersAsync(target.PaymentPartnerId,
            source.PaymentPartnerId, CancellationToken.None);

        // Assert
        merge.IsSuccess.Should().BeTrue();
        merge.Value.Should().Be(1, "one transaction pointed to the source partner");

        var partners = await DbContext.PaymentPartners.AsNoTracking()
            .Where(p => p.PersonId == ExecutionContext.PersonId)
            .ToListAsync();
        partners.Should().HaveCount(1);
        partners[0].PaymentPartnerId.Should().Be(target.PaymentPartnerId);
        partners[0].Iban.Should().Be("DE44500105175407324931");
        partners[0].LinkedAccountId.Should().Be(savings.AccountId);
        partners[0].NormalizedName.Should().Be("TAGESGELD");

        (await DbContext.Transactions.AsNoTracking()
                .CountAsync(t => t.PaymentPartnerId == target.PaymentPartnerId))
            .Should().Be(2);
    }

    [Fact]
    public async Task Merge_ShouldRejectSelfMergeAndUnknownIds()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [Parsed(new DateOnly(2026, 3, 1), -10.00m, "Kiosk Özdemir", null)],
            TransactionSource.CsvImport, CancellationToken.None);

        var partner = await DbContext.PaymentPartners
            .SingleAsync(p => p.PersonId == ExecutionContext.PersonId);

        // Act
        var selfMerge = await PaymentPartnerCommands.MergePaymentPartnersAsync(partner.PaymentPartnerId,
            partner.PaymentPartnerId, CancellationToken.None);
        var unknown = await PaymentPartnerCommands.MergePaymentPartnersAsync(partner.PaymentPartnerId,
            999_999, CancellationToken.None);

        // Assert
        selfMerge.IsFailure.Should().BeTrue();
        unknown.IsFailure.Should().BeTrue();
        unknown.Error.Should().Be(
            FinanceErrors.PaymentPartnerMergeFailedWithMessage("PaymentPartnerId is invalid"));
    }

    [Fact]
    public async Task GetPaymentPartners_ShouldReturnOnlyOwnWithStats()
    {
        // Arrange: two own partners with bookings, one partner of a stranger
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [
                Parsed(new DateOnly(2026, 3, 5), -3.50m, "Bäcker Schmidt", null),
                Parsed(new DateOnly(2026, 4, 5), -4.20m, "Bäcker Schmidt", null),
                Parsed(new DateOnly(2026, 3, 7), -12.99m, "Apotheke am Markt", null)
            ],
            TransactionSource.CsvImport, CancellationToken.None);

        var stranger = await PeopleDataSeeder.SeedPersonAsync();
        DbContext.PaymentPartners.Add(new PaymentPartner
        {
            PersonId = stranger.PersonId,
            DisplayName = "Fremder Laden",
            NormalizedName = "FREMDER LADEN",
            CreatedById = stranger.PersonId
        });
        await DbContext.SaveChangesAsync();

        // Act
        var result = await PaymentPartnerQueries.GetPaymentPartnersAsync(CancellationToken.None);

        // Assert: only own partners, ordered by display name, with usage stats
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value[0].Partner.DisplayName.Should().Be("Apotheke am Markt");
        result.Value[1].Partner.DisplayName.Should().Be("Bäcker Schmidt");
        result.Value[1].TransactionCount.Should().Be(2);
        result.Value[1].LastBookingDate.Should().Be(new DateOnly(2026, 4, 5));
    }

    [Fact]
    public async Task GetTransactions_ShouldFilterByPaymentPartnerId()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            [
                Parsed(new DateOnly(2026, 3, 5), -3.50m, "Bäcker Schmidt", null),
                Parsed(new DateOnly(2026, 4, 5), -4.20m, "Bäcker Schmidt", null),
                Parsed(new DateOnly(2026, 3, 7), -12.99m, "Apotheke am Markt", null)
            ],
            TransactionSource.CsvImport, CancellationToken.None);

        var baecker = await DbContext.PaymentPartners
            .SingleAsync(p => p.PersonId == ExecutionContext.PersonId && p.NormalizedName == "BÄCKER SCHMIDT");

        // Act
        var filtered = await TransactionQueries.GetTransactionsAsync(account.AccountId, null, null, null,
            null, null, baecker.PaymentPartnerId, 1, 50, CancellationToken.None);

        // Assert
        filtered.IsSuccess.Should().BeTrue();
        filtered.Value.TotalCount.Should().Be(2);
        filtered.Value.Items.Should().OnlyContain(t => t.PaymentPartnerId == baecker.PaymentPartnerId);
    }

    [Fact]
    public async Task UpdateAccount_ShouldPersistDeactivatedFrom()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);

        // Act
        var result = await AccountCommands.UpdateAccountAsync(new Account
        {
            AccountId = account.AccountId,
            Name = account.Name,
            AccountType = account.AccountType,
            CurrencyCode = account.CurrencyCode,
            IsActive = false,
            DeactivatedFrom = new DateOnly(2026, 6, 30)
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var updated = await DbContext.Accounts.AsNoTracking()
            .SingleAsync(a => a.AccountId == account.AccountId);
        updated.IsActive.Should().BeFalse();
        updated.DeactivatedFrom.Should().Be(new DateOnly(2026, 6, 30));
    }
}
