using System.Text;
using Domain.Entities.Finance;
using Infrastructure.Features.Finance.Import;
using Microsoft.EntityFrameworkCore;

namespace ApplicationTests.IntegrationTests.Finance;

public class ImportTransactionsTests : BaseFinanceCommandsTest
{
    private const string SparkasseCsv =
        """
        "Auftragskonto";"Buchungstag";"Valutadatum";"Buchungstext";"Verwendungszweck";"Beguenstigter/Zahlungspflichtiger";"Kontonummer/IBAN";"BIC (SWIFT-Code)";"Betrag";"Waehrung";"Info"
        "DE12345678901234567890";"02.03.26";"02.03.26";"KARTENZAHLUNG";"Einkauf Lebensmittel";"REWE Markt";"DE89370400440532013000";"COBADEFFXXX";"-49,99";"EUR";"Umsatz gebucht"
        "DE12345678901234567890";"05.03.26";"05.03.26";"LASTSCHRIFT";"Miete März";"Vermieter Müller";"DE02120300000000202051";"BYLADEM1001";"-950,00";"EUR";"Umsatz gebucht"
        "DE12345678901234567890";"05.03.26";"05.03.26";"LASTSCHRIFT";"Miete März";"Vermieter Müller";"DE02120300000000202051";"BYLADEM1001";"-950,00";"EUR";"Umsatz gebucht"
        """;

    private const string Camt053Xml =
        """
        <?xml version="1.0" encoding="UTF-8"?>
        <Document xmlns="urn:iso:std:iso:20022:tech:xsd:camt.053.001.08">
          <BkToCstmrStmt>
            <Stmt>
              <Ntry>
                <NtryRef>REF-1</NtryRef>
                <Amt Ccy="EUR">49.99</Amt>
                <CdtDbtInd>DBIT</CdtDbtInd>
                <BookgDt><Dt>2026-03-02</Dt></BookgDt>
                <ValDt><Dt>2026-03-02</Dt></ValDt>
                <NtryDtls>
                  <TxDtls>
                    <Refs><EndToEndId>E2E-1</EndToEndId></Refs>
                    <RltdPties>
                      <Cdtr><Nm>REWE Markt</Nm></Cdtr>
                      <CdtrAcct><Id><IBAN>DE89370400440532013000</IBAN></Id></CdtrAcct>
                    </RltdPties>
                    <RmtInf><Ustrd>Einkauf Lebensmittel</Ustrd></RmtInf>
                  </TxDtls>
                </NtryDtls>
              </Ntry>
              <Ntry>
                <Amt Ccy="EUR">2500.00</Amt>
                <CdtDbtInd>CRDT</CdtDbtInd>
                <BookgDt><Dt>2026-03-01</Dt></BookgDt>
                <NtryDtls>
                  <TxDtls>
                    <RltdPties>
                      <Dbtr><Nm>Arbeitgeber GmbH</Nm></Dbtr>
                      <DbtrAcct><Id><IBAN>DE44500105175407324931</IBAN></Id></DbtrAcct>
                    </RltdPties>
                    <RmtInf><Ustrd>Gehalt Maerz</Ustrd></RmtInf>
                  </TxDtls>
                </NtryDtls>
              </Ntry>
            </Stmt>
          </BkToCstmrStmt>
        </Document>
        """;

    public ImportTransactionsTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task Import_ShouldImportSparkasseCsvIncludingInFileDuplicates()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var parser = new SparkasseCamtCsvParser();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SparkasseCsv));

        // Act
        var parseResult = parser.Parse(stream);
        var importResult = await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            parseResult.Value.Transactions, parser.Source, CancellationToken.None);

        // Assert
        parseResult.IsSuccess.Should().BeTrue();
        parseResult.Value.Errors.Should().BeEmpty();
        parseResult.Value.Transactions.Should().HaveCount(3);

        importResult.IsSuccess.Should().BeTrue();
        importResult.Value.Imported.Should().Be(3);
        importResult.Value.SkippedDuplicates.Should().Be(0);

        var imported = await DbContext.Transactions.AsNoTracking()
            .Where(t => t.AccountId == account.AccountId)
            .ToListAsync();
        imported.Should().HaveCount(3);
        imported.Should().Contain(t =>
            t.Amount == -49.99m &&
            t.BookingDate == new DateOnly(2026, 3, 2) &&
            t.CounterpartyName == "REWE Markt" &&
            t.CounterpartyIban == "DE89370400440532013000");
        imported.Count(t => t.Amount == -950.00m).Should().Be(2);
    }

    [Fact]
    public async Task Import_ShouldSkipAllOnReimport()
    {
        // Arrange
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var parser = new SparkasseCamtCsvParser();

        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(SparkasseCsv)))
        {
            var first = parser.Parse(stream);
            await TransactionCommands.ImportTransactionsAsync(account.AccountId, first.Value.Transactions,
                parser.Source, CancellationToken.None);
        }

        // Act
        using var reimportStream = new MemoryStream(Encoding.UTF8.GetBytes(SparkasseCsv));
        var reimportParse = parser.Parse(reimportStream);
        var reimport = await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            reimportParse.Value.Transactions, parser.Source, CancellationToken.None);

        // Assert
        reimport.IsSuccess.Should().BeTrue();
        reimport.Value.Imported.Should().Be(0);
        reimport.Value.SkippedDuplicates.Should().Be(3);
        (await DbContext.Transactions.CountAsync(t => t.AccountId == account.AccountId)).Should().Be(3);
    }

    [Fact]
    public async Task Import_ShouldDedupAcrossCsvAndCamtFormats()
    {
        // Arrange: the CAMT file contains the same REWE booking as the CSV plus a new salary entry
        var account = await FinanceDataSeeder.GenereateDummyAccount(ExecutionContext.PersonId);
        var csvParser = new SparkasseCamtCsvParser();
        var camtParser = new Camt053Parser();

        using (var csvStream = new MemoryStream(Encoding.UTF8.GetBytes(SparkasseCsv)))
        {
            var csvParse = csvParser.Parse(csvStream);
            await TransactionCommands.ImportTransactionsAsync(account.AccountId, csvParse.Value.Transactions,
                csvParser.Source, CancellationToken.None);
        }

        // Act
        using var camtStream = new MemoryStream(Encoding.UTF8.GetBytes(Camt053Xml));
        var camtParse = camtParser.Parse(camtStream);
        var camtImport = await TransactionCommands.ImportTransactionsAsync(account.AccountId,
            camtParse.Value.Transactions, camtParser.Source, CancellationToken.None);

        // Assert
        camtParse.IsSuccess.Should().BeTrue();
        camtParse.Value.Transactions.Should().HaveCount(2);

        camtImport.IsSuccess.Should().BeTrue();
        camtImport.Value.Imported.Should().Be(1);
        camtImport.Value.SkippedDuplicates.Should().Be(1);

        var salary = await DbContext.Transactions.AsNoTracking()
            .SingleAsync(t => t.AccountId == account.AccountId && t.Amount == 2500.00m);
        salary.CounterpartyName.Should().Be("Arbeitgeber GmbH");
        salary.CounterpartyIban.Should().Be("DE44500105175407324931");
        salary.BookingDate.Should().Be(new DateOnly(2026, 3, 1));
    }

    [Fact]
    public async Task Import_ShouldReturnErrorWhenNotOwner()
    {
        // Arrange
        var otherPerson = await PeopleDataSeeder.SeedPersonAsync();
        var foreignAccount = await FinanceDataSeeder.GenereateDummyAccount(otherPerson.PersonId);
        var parser = new SparkasseCamtCsvParser();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(SparkasseCsv));
        var parseResult = parser.Parse(stream);

        // Act
        var result = await TransactionCommands.ImportTransactionsAsync(foreignAccount.AccountId,
            parseResult.Value.Transactions, parser.Source, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(FinanceErrors.ImportFailedWithMessage("AccountId is invalid"));
    }

    [Fact]
    public void Parse_ShouldHandleWindows1252Encoding()
    {
        // Arrange
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var parser = new SparkasseCamtCsvParser();
        using var stream = new MemoryStream(Encoding.GetEncoding(1252).GetBytes(SparkasseCsv));

        // Act
        var result = parser.Parse(stream);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Errors.Should().BeEmpty();
        result.Value.Transactions.Should().Contain(t =>
            t.Purpose == "Miete März" && t.CounterpartyName == "Vermieter Müller");
    }

    [Fact]
    public void CanParse_ShouldDetectFormats()
    {
        // Arrange
        var csvParser = new SparkasseCamtCsvParser();
        var camtParser = new Camt053Parser();
        var csvHead = Encoding.UTF8.GetBytes(SparkasseCsv)[..256];
        var camtHead = Encoding.UTF8.GetBytes(Camt053Xml)[..256];

        // Assert
        csvParser.CanParse("umsaetze.csv", csvHead).Should().BeTrue();
        csvParser.CanParse("statement.xml", camtHead).Should().BeFalse();
        camtParser.CanParse("statement.xml", camtHead).Should().BeTrue();
    }
}
