using System.Text;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Infrastructure.Features.Finance.Import;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Finance;

public class RevolutImportTests : BaseFinanceCommandsTest
{
    // Shape of a Revolut "account-statement_…_de-de_….csv" export; the two identical Apple Pay
    // top-ups on the same day exercise the in-file occurrence dedup
    private const string RevolutGermanCsv =
        """
        Art,Produkt,Datum des Beginns,Datum des Abschlusses,Beschreibung,Betrag,Gebühr,Währung,Status,Kontostand
        Einzahlung,Giro,2025-09-24 19:58:56,2025-09-24 19:58:57,Apple Pay Einzahlung von *9555,10.00,0.00,EUR,ABGESCHLOSSEN,10.00
        Einzahlung,Giro,2025-09-24 20:22:32,2025-09-24 20:22:33,Apple Pay Einzahlung von *9555,10.00,0.00,EUR,ABGESCHLOSSEN,20.00
        Transfer,Giro,2025-09-24 19:59:30,2025-09-24 19:59:30,Überweisung an VIKTORIA SCHIFFMANN,-1.00,0.00,EUR,ABGESCHLOSSEN,9.00
        Gebühr,Giro,2025-09-28 09:51:45,2025-09-28 09:51:45,Kartenlieferungsgebühr,-7.99,0.00,EUR,ABGESCHLOSSEN,78.02
        Kartenbezahlung,Giro,2025-10-04 08:58:08,2025-10-05 06:07:39,pappert,-2.84,0.50,EUR,ABGESCHLOSSEN,225.18
        Transfer,Giro,2025-10-04 18:43:06,2025-10-04 18:43:06,An EUR Tagesgeld,-300.00,0.00,EUR,REVERTED,228.02
        """;

    private const string RevolutEnglishCsv =
        """
        Type,Product,Started Date,Completed Date,Description,Amount,Fee,Currency,State,Balance
        TOPUP,Current,2025-09-24 19:58:56,2025-09-24 19:58:57,Apple Pay Top-Up by *9555,10.00,0.00,EUR,COMPLETED,10.00
        """;

    public RevolutImportTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public void Parse_ShouldParseGermanExportWithFeeAndStatusFilter()
    {
        // Arrange
        var parser = new RevolutCsvParser();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(RevolutGermanCsv));

        // Act
        var result = parser.Parse(stream);

        // Assert: 5 completed rows + 1 fee booking; the REVERTED row is skipped
        result.IsSuccess.Should().BeTrue();
        result.Value.Errors.Should().BeEmpty();
        result.Value.Transactions.Should().HaveCount(6);

        result.Value.Transactions.Count(t => t.Amount == 10.00m).Should().Be(2);
        result.Value.Transactions.Should().NotContain(t => t.Amount == -300.00m);

        // Booking date = completed date, value date = started date
        var cardPayment = result.Value.Transactions.Single(t => t.Amount == -2.84m);
        cardPayment.BookingDate.Should().Be(new DateOnly(2025, 10, 5));
        cardPayment.ValueDate.Should().Be(new DateOnly(2025, 10, 4));
        cardPayment.Purpose.Should().Be("pappert");

        // Fee > 0 yields a separately categorizable booking
        var fee = result.Value.Transactions.Single(t => t.Amount == -0.50m);
        fee.BookingDate.Should().Be(new DateOnly(2025, 10, 5));
        fee.Purpose.Should().Be("Gebühr: pappert");
    }

    [Fact]
    public void Parse_ShouldParseEnglishHeaders()
    {
        // Arrange
        var parser = new RevolutCsvParser();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(RevolutEnglishCsv));

        // Act
        var result = parser.Parse(stream);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Transactions.Should().ContainSingle();
        result.Value.Transactions[0].Amount.Should().Be(10.00m);
        result.Value.Transactions[0].BookingDate.Should().Be(new DateOnly(2025, 9, 24));
    }

    [Fact]
    public async Task Import_ShouldImportIncludingInFileDuplicatesAndDedupOnReimport()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var parser = new RevolutCsvParser();

        // Act: first import
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(RevolutGermanCsv)))
        {
            var parse = parser.Parse(stream);
            var import = await TransactionCommands.ImportTransactionsAsync(account.AccountId,
                parse.Value.Transactions, parser.Source, CancellationToken.None);

            import.IsSuccess.Should().BeTrue();
            import.Value.Imported.Should().Be(6);
            import.Value.SkippedDuplicates.Should().Be(0);
        }

        // Act: re-import of the same file
        using var reimportStream = new MemoryStream(Encoding.UTF8.GetBytes(RevolutGermanCsv));
        var reimportParse = parser.Parse(reimportStream);
        var reimport = await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            reimportParse.Value.Transactions, parser.Source, CancellationToken.None);

        // Assert
        reimport.Value.Imported.Should().Be(0);
        reimport.Value.SkippedDuplicates.Should().Be(6);

        var imported = await DbContext.Transactions.AsNoTracking()
            .Where(t => t.AccountId == account.AccountId)
            .ToListAsync();
        imported.Should().HaveCount(6);
        imported.Should().OnlyContain(t => t.Source == TransactionSource.CsvImport);
        imported.Count(t => t.Amount == 10.00m && t.BookingDate == new DateOnly(2025, 9, 24)).Should().Be(2);
    }

    [Fact]
    public void CanParse_ShouldDetectRevolutWithoutClaimingSparkasse()
    {
        // Arrange
        var revolutParser = new RevolutCsvParser();
        var sparkasseParser = new SparkasseCamtCsvParser();
        var revolutHead = Encoding.UTF8.GetBytes(RevolutGermanCsv)[..256];
        var englishHead = Encoding.UTF8.GetBytes(RevolutEnglishCsv)[..128];
        var sparkasseHead = Encoding.UTF8.GetBytes(
            "\"Auftragskonto\";\"Buchungstag\";\"Valutadatum\";\"Betrag\";\"Waehrung\"");

        // Assert
        revolutParser.CanParse("account-statement_2025-09-23_2026-07-12_de-de_930fca.csv", revolutHead)
            .Should().BeTrue();
        revolutParser.CanParse("statement.csv", englishHead).Should().BeTrue();
        revolutParser.CanParse("umsaetze.csv", sparkasseHead).Should().BeFalse();
        sparkasseParser.CanParse("account-statement.csv", revolutHead).Should().BeFalse();
    }
}
