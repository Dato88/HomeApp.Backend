using System.Globalization;
using System.Text;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Infrastructure.Features.Finance.Import;

// Revolut account statement export ("account-statement_…​.csv"): comma separated, UTF-8,
// ISO-like timestamps (yyyy-MM-dd HH:mm:ss), decimal point amounts, German or English headers.
// Only completed rows are imported; a fee > 0 yields a second, separately categorizable booking.
public sealed class RevolutCsvParser : IBankStatementParser
{
    private static readonly string[] DateFormats = ["yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd"];
    private static readonly string[] CompletedStates = ["ABGESCHLOSSEN", "COMPLETED"];

    public string FormatName => "revolut-csv";
    public TransactionSource Source => TransactionSource.CsvImport;

    public bool CanParse(string fileName, byte[] head)
    {
        var text = Encoding.UTF8.GetString(head);
        return (text.Contains("Datum des Beginns", StringComparison.OrdinalIgnoreCase) ||
                text.Contains("Started Date", StringComparison.OrdinalIgnoreCase)) &&
               text.Contains(',', StringComparison.Ordinal);
    }

    public Result<BankStatementParseResult> Parse(Stream content)
    {
        using var reader = new StreamReader(content, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var lines = reader.ReadToEnd()
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.TrimEnd('\r'))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

        if (lines.Count < 1)
            return Result.Failure<BankStatementParseResult>(
                FinanceErrors.ImportFailedWithMessage("CSV file is empty"));

        var header = CsvLineSplitter.Split(lines[0], ',');
        var columns = header
            .Select((name, i) => (name: name.Trim(), i))
            .ToDictionary(x => x.name, x => x.i, StringComparer.OrdinalIgnoreCase);

        var completedDateIndex = FindColumn(columns, "Datum des Abschlusses", "Completed Date");
        var amountIndex = FindColumn(columns, "Betrag", "Amount");

        if (completedDateIndex is null || amountIndex is null)
            return Result.Failure<BankStatementParseResult>(FinanceErrors.ImportFailedWithMessage(
                "CSV header is missing Datum des Abschlusses/Completed Date or Betrag/Amount column"));

        var startedDateIndex = FindColumn(columns, "Datum des Beginns", "Started Date");
        var descriptionIndex = FindColumn(columns, "Beschreibung", "Description");
        var feeIndex = FindColumn(columns, "Gebühr", "Fee");
        var currencyIndex = FindColumn(columns, "Währung", "Currency");
        var stateIndex = FindColumn(columns, "Status", "State");

        var transactions = new List<ParsedTransaction>();
        var errors = new List<string>();

        for (var lineNumber = 1; lineNumber < lines.Count; lineNumber++)
        {
            var fields = CsvLineSplitter.Split(lines[lineNumber], ',');

            try
            {
                // Pending/reverted rows are no final account movements
                var state = GetField(fields, stateIndex);
                if (state is not null && !CompletedStates.Contains(state, StringComparer.OrdinalIgnoreCase))
                    continue;

                var startedDate = TryParseDate(GetField(fields, startedDateIndex));
                var bookingDate = TryParseDate(GetField(fields, completedDateIndex)) ?? startedDate
                    ?? throw new FormatException("Missing completed and started date");

                var amount = ParseAmount(GetField(fields, amountIndex))
                             ?? throw new FormatException("Missing amount");
                var fee = ParseAmount(GetField(fields, feeIndex)) ?? 0m;
                var description = EmptyToNull(GetField(fields, descriptionIndex));
                var currency = EmptyToNull(GetField(fields, currencyIndex));

                transactions.Add(new ParsedTransaction(
                    bookingDate,
                    startedDate,
                    amount,
                    currency,
                    null,
                    null,
                    description,
                    null));

                // Revolut lists fees separately from the amount; a distinct booking keeps them
                // independently categorizable (and the purpose prefix yields a distinct import hash)
                if (fee > 0)
                    transactions.Add(new ParsedTransaction(
                        bookingDate,
                        startedDate,
                        -fee,
                        currency,
                        null,
                        null,
                        $"Gebühr: {description}",
                        null));
            }
            catch (Exception ex)
            {
                errors.Add($"Line {lineNumber + 1}: {ex.Message}");
            }
        }

        return Result.Success(new BankStatementParseResult(transactions, errors));
    }

    private static int? FindColumn(Dictionary<string, int> columns, params string[] names)
    {
        foreach (var name in names)
        {
            if (columns.TryGetValue(name, out var index))
                return index;
        }

        return null;
    }

    private static string? GetField(List<string> fields, int? index) =>
        index.HasValue && index.Value < fields.Count ? fields[index.Value].Trim() : null;

    private static string? EmptyToNull(string? value) => string.IsNullOrWhiteSpace(value) ? null : value;

    private static DateOnly? TryParseDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        foreach (var format in DateFormats)
        {
            if (DateTime.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var dateTime))
                return DateOnly.FromDateTime(dateTime);
        }

        return null;
    }

    private static decimal? ParseAmount(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : decimal.Parse(value, NumberStyles.Number, CultureInfo.InvariantCulture);
}
