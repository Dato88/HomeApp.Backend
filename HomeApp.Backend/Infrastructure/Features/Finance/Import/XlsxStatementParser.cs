using System.Globalization;
using Application.Abstractions.FinanceModule;
using ClosedXML.Excel;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Infrastructure.Features.Finance.Import;

// Excel (.xlsx) statement or self-maintained transaction list: first worksheet, header row with
// the same German column names as the Sparkasse CSV export. Cells may be native Excel dates and
// numbers or German formatted text (dd.MM.yy dates, decimal comma).
public sealed class XlsxStatementParser : IBankStatementParser
{
    private static readonly string[] DateFormats = ["dd.MM.yy", "dd.MM.yyyy"];
    private static readonly byte[] ZipMagic = [0x50, 0x4B, 0x03, 0x04];

    public string FormatName => "xlsx";
    public TransactionSource Source => TransactionSource.XlsxImport;

    // xlsx = ZIP container. ZIP magic alone is enough here: the import validator only lets
    // .csv/.xml/.txt/.xlsx through and the CSV/CAMT parsers are probed first, so a ZIP that is
    // no workbook just fails in Parse with a clear error.
    public bool CanParse(string fileName, byte[] head) =>
        fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase) ||
        (head.Length >= ZipMagic.Length && head.AsSpan(0, ZipMagic.Length).SequenceEqual(ZipMagic));

    public Result<BankStatementParseResult> Parse(Stream content)
    {
        try
        {
            using var workbook = new XLWorkbook(content);
            var worksheet = workbook.Worksheets.FirstOrDefault();

            if (worksheet is null)
                return Result.Failure<BankStatementParseResult>(
                    FinanceErrors.ImportFailedWithMessage("XLSX workbook has no worksheet"));

            var headerRow = worksheet.FirstRowUsed();

            if (headerRow is null)
                return Result.Failure<BankStatementParseResult>(
                    FinanceErrors.ImportFailedWithMessage("XLSX worksheet is empty"));

            var columns = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var cell in headerRow.CellsUsed())
                columns.TryAdd(cell.GetString().Trim(), cell.Address.ColumnNumber);

            var bookingDateColumn = FindColumn(columns, "Buchungstag");
            var amountColumn = FindColumn(columns, "Betrag");

            if (bookingDateColumn is null || amountColumn is null)
                return Result.Failure<BankStatementParseResult>(
                    FinanceErrors.ImportFailedWithMessage("XLSX header is missing Buchungstag or Betrag column"));

            var valueDateColumn = FindColumn(columns, "Valutadatum");
            var purposeColumn = FindColumn(columns, "Verwendungszweck");
            var counterpartyColumn = FindColumn(columns, "Beguenstigter/Zahlungspflichtiger",
                "Begünstigter/Zahlungspflichtiger");
            var ibanColumn = FindColumn(columns, "Kontonummer/IBAN", "IBAN");
            var referenceColumn = FindColumn(columns, "Kundenreferenz (End-to-End)", "Kundenreferenz");
            var currencyColumn = FindColumn(columns, "Waehrung", "Währung");

            var transactions = new List<ParsedTransaction>();
            var errors = new List<string>();

            foreach (var row in worksheet.RowsUsed())
            {
                if (row.RowNumber() == headerRow.RowNumber())
                    continue;

                try
                {
                    var bookingDate = GetDate(row, bookingDateColumn)
                                      ?? throw new FormatException("Invalid or missing booking date");
                    var amount = GetAmount(row, amountColumn.Value);

                    var reference = GetText(row, referenceColumn);
                    if (string.Equals(reference, "NOTPROVIDED", StringComparison.OrdinalIgnoreCase))
                        reference = null;

                    transactions.Add(new ParsedTransaction(
                        bookingDate,
                        GetDate(row, valueDateColumn),
                        amount,
                        GetText(row, currencyColumn),
                        GetText(row, counterpartyColumn),
                        GetText(row, ibanColumn),
                        GetText(row, purposeColumn),
                        reference));
                }
                catch (Exception ex)
                {
                    errors.Add($"Row {row.RowNumber()}: {ex.Message}");
                }
            }

            return Result.Success(new BankStatementParseResult(transactions, errors));
        }
        catch (Exception ex)
        {
            return Result.Failure<BankStatementParseResult>(
                FinanceErrors.ImportFailedWithMessage($"File is not a readable xlsx workbook: {ex.Message}"));
        }
    }

    private static int? FindColumn(Dictionary<string, int> columns, params string[] names)
    {
        foreach (var name in names)
        {
            if (columns.TryGetValue(name, out var column))
                return column;
        }

        return null;
    }

    private static DateOnly? GetDate(IXLRow row, int? column)
    {
        if (!column.HasValue)
            return null;

        var cell = row.Cell(column.Value);

        if (cell.DataType == XLDataType.DateTime)
            return DateOnly.FromDateTime(cell.GetDateTime());

        var text = cell.GetString().Trim();

        if (string.IsNullOrWhiteSpace(text))
            return null;

        foreach (var format in DateFormats)
        {
            if (DateOnly.TryParseExact(text, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var date))
                return date;
        }

        return null;
    }

    private static decimal GetAmount(IXLRow row, int column)
    {
        var cell = row.Cell(column);

        if (cell.DataType == XLDataType.Number)
            return cell.GetValue<decimal>();

        return decimal.Parse(cell.GetString().Trim(), NumberStyles.Number, CultureInfo.GetCultureInfo("de-DE"));
    }

    private static string? GetText(IXLRow row, int? column)
    {
        if (!column.HasValue)
            return null;

        var text = row.Cell(column.Value).GetString().Trim();

        return string.IsNullOrWhiteSpace(text) ? null : text;
    }
}
