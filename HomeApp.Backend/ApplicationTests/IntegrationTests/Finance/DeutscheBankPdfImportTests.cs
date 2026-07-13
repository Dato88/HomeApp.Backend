using System.Text;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using Infrastructure.Features.Finance.Import;
using Microsoft.EntityFrameworkCore;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Writer;

namespace ApplicationTests.IntegrationTests.Finance;

public class DeutscheBankPdfImportTests : BaseFinanceCommandsTest
{
    // Embedded TrueType font so the test PDFs can carry real German text incl. umlauts
    private static readonly string FontPath = Path.Combine(AppContext.BaseDirectory,
        "IntegrationTests", "TestData", "Fonts", "OpenSans-Regular.ttf");

    public DeutscheBankPdfImportTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public void Parse_ShouldReconstructEntriesFromColumnLayout()
    {
        // Arrange
        var parser = new DeutscheBankPdfParser();
        using var stream = new MemoryStream(BuildStatementPdf());

        // Act
        var result = parser.Parse(stream);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Errors.Should().BeEmpty();
        result.Value.Transactions.Should().HaveCount(4);

        // SEPA Lastschrift: purpose without the SEPA metadata noise
        var direktDebit = result.Value.Transactions.Single(t => t.Amount == -10.00m);
        direktDebit.BookingDate.Should().Be(new DateOnly(2026, 3, 2));
        direktDebit.ValueDate.Should().Be(new DateOnly(2026, 3, 2));
        direktDebit.CounterpartyName.Should().Be("Bayern Versicherung Lebensversicherung AG");
        direktDebit.Purpose.Should().Contain("LEBEN LV-1-0454-2814");
        direktDebit.Purpose.Should().Contain("P82-518038441014");
        direktDebit.Purpose.Should().NotContain("Gläubiger-ID");
        direktDebit.Purpose.Should().NotContain("RCUR");

        // Überweisung: counterparty IBAN captured from its own line
        var transfer = result.Value.Transactions.Single(t => t.Amount == -16.65m);
        transfer.BookingDate.Should().Be(new DateOnly(2026, 3, 13));
        transfer.CounterpartyName.Should().Be("Andrej Miller");
        transfer.CounterpartyIban.Should().Be("DE12793530900011094851");
        transfer.Purpose.Should().StartWith("Kontoauflösung");

        // Unsigned Haben amount: the right-aligned column decides the sign
        var credit = result.Value.Transactions.Single(t => t.Amount == 2500.00m);
        credit.BookingDate.Should().Be(new DateOnly(2026, 3, 15));
        credit.CounterpartyName.Should().Be("Arbeitgeber GmbH");
        credit.Purpose.Should().Be("Gehalt Maerz");

        // KONTOABRECHNUNG: marker on the entry line itself, no counterparty
        var settlement = result.Value.Transactions.Single(t => t.Amount == -10.76m);
        settlement.CounterpartyName.Should().BeNull();
        settlement.Purpose.Should().Be("KONTOABRECHNUNG");
    }

    [Fact]
    public void Parse_ShouldReturnErrorWhenNoStatementTableExists()
    {
        // Arrange
        var parser = new DeutscheBankPdfParser();
        var builder = new PdfDocumentBuilder();
        var font = builder.AddTrueTypeFont(File.ReadAllBytes(FontPath));
        builder.AddPage(PageSize.A4).AddText("Nur ein Brief, kein Kontoauszug", 10, new PdfPoint(50, 700), font);

        // Act
        using var stream = new MemoryStream(builder.Build());
        var result = parser.Parse(stream);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.ImportFailedWithMessage(
            "No statement table (Buchung/Valuta/Vorgang/Soll/Haben) found in PDF"));
    }

    [Fact]
    public async Task Import_ShouldImportPdfAndDedupOnReimport()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var parser = new DeutscheBankPdfParser();
        var bytes = BuildStatementPdf();

        // Act: first import
        using (var stream = new MemoryStream(bytes))
        {
            var parse = parser.Parse(stream);
            var import = await TransactionCommands.ImportTransactionsAsync(account.AccountId,
                parse.Value.Transactions, parser.Source, CancellationToken.None);

            import.IsSuccess.Should().BeTrue();
            import.Value.Imported.Should().Be(4);
        }

        // Act: re-import of the same statement
        using var reimportStream = new MemoryStream(bytes);
        var reimportParse = parser.Parse(reimportStream);
        var reimport = await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            reimportParse.Value.Transactions, parser.Source, CancellationToken.None);

        // Assert
        reimport.Value.Imported.Should().Be(0);
        reimport.Value.SkippedDuplicates.Should().Be(4);

        var imported = await DbContext.Transactions.AsNoTracking()
            .Where(t => t.AccountId == account.AccountId)
            .ToListAsync();
        imported.Should().HaveCount(4);
        imported.Should().OnlyContain(t => t.Source == TransactionSource.PdfImport);
    }

    [Fact]
    public void CanParse_ShouldDetectPdfWithoutClaimingOtherFormats()
    {
        // Arrange
        var pdfParser = new DeutscheBankPdfParser();
        var xlsxParser = new XlsxStatementParser();
        var bytes = BuildStatementPdf();
        var head = bytes[..Math.Min(512, bytes.Length)];

        // Assert: by extension, by %PDF magic, and no cross-claiming
        pdfParser.CanParse("Kontoauszug_228087773805EUR.pdf", head).Should().BeTrue();
        pdfParser.CanParse("auszug.dat", head).Should().BeTrue();
        pdfParser.CanParse("umsaetze.csv", Encoding.UTF8.GetBytes("\"Auftragskonto\";\"Betrag\"")).Should().BeFalse();
        xlsxParser.CanParse("auszug.dat", head).Should().BeFalse();
    }

    // Builds a minimal Deutsche Bank "Kontoauszug" lookalike: words placed at the real column
    // positions so the coordinate-based parser is exercised like with a genuine statement
    private static byte[] BuildStatementPdf()
    {
        var builder = new PdfDocumentBuilder();
        var font = builder.AddTrueTypeFont(File.ReadAllBytes(FontPath));
        var page = builder.AddPage(PageSize.A4);

        void Text(double x, double y, string text) => page.AddText(text, 8, new PdfPoint(x, y), font);

        // Column header
        Text(50, 700, "Buchung");
        Text(95, 700, "Valuta");
        Text(140, 700, "Vorgang");
        Text(450, 700, "Soll");
        Text(520, 700, "Haben");

        // Entry 1: SEPA direct debit with multi-line purpose and SEPA metadata noise
        Text(50, 685, "02.03.");
        Text(95, 685, "02.03.");
        Text(140, 685, "SEPA Lastschrifteinzug von");
        Text(430, 685, "- 10,00");
        Text(50, 675, "2026");
        Text(95, 675, "2026");
        Text(140, 675, "Bayern Versicherung Lebensversicherung AG");
        Text(140, 665, "Verwendungszweck/ Kundenreferenz");
        Text(140, 655, "LEBEN LV-1-0454-2814 Miller Andrej01.03.2026");
        Text(140, 645, "P82-518038441014");
        Text(140, 635, "Gläubiger-ID DE61BL000000156981");
        Text(140, 625, "Mand-ID LV000104542814");
        Text(140, 615, "ABWA Versicherungskammer Bayern");
        Text(140, 605, "RCUR Wiederholungslastschrift");

        // Entry 2: transfer with counterparty IBAN/BIC lines
        Text(50, 590, "13.03.");
        Text(95, 590, "13.03.");
        Text(140, 590, "SEPA Überweisung an");
        Text(430, 590, "- 16,65");
        Text(50, 580, "2026");
        Text(95, 580, "2026");
        Text(140, 580, "Andrej Miller");
        Text(140, 570, "IBAN DE12793530900011094851");
        Text(140, 560, "BIC BYLADEM1NES");
        Text(140, 550, "Verwendungszweck/ Kundenreferenz");
        Text(140, 540, "Kontoauflösung 228 0877738 05 per 13.03.2026");

        // Entry 3: credit without printed sign, right-aligned under the Haben column
        Text(50, 525, "15.03.");
        Text(95, 525, "15.03.");
        Text(140, 525, "Gutschrift");
        Text(505, 525, "2.500,00");
        Text(50, 515, "2026");
        Text(95, 515, "2026");
        Text(140, 515, "Arbeitgeber GmbH");
        Text(140, 505, "Verwendungszweck/ Kundenreferenz");
        Text(140, 495, "Gehalt Maerz");

        // Entry 4: KONTOABRECHNUNG (purpose marker directly on the entry line)
        Text(50, 480, "13.03.");
        Text(95, 480, "13.03.");
        Text(140, 480, "Verwendungszweck/ Kundenreferenz");
        Text(430, 480, "- 10,76");
        Text(50, 470, "2026");
        Text(95, 470, "2026");
        Text(140, 470, "KONTOABRECHNUNG");

        // Everything below the stop marker must be ignored
        Text(50, 450, "Neuer Saldo");
        Text(400, 450, "EUR + 0,00");

        return builder.Build();
    }
}
