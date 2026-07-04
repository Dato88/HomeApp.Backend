using System.Globalization;
using System.Text;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Infrastructure.Features.Finance.Import;

// German Sparkasse "CSV-CAMT" export: semicolon separated, quoted fields, dd.MM.yy dates,
// decimal comma, typically Windows-1252 encoded.
public sealed class SparkasseCamtCsvParser : IBankStatementParser
{
    private static readonly string[] DateFormats = ["dd.MM.yy", "dd.MM.yyyy"];

    static SparkasseCamtCsvParser() => Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

    public string FormatName => "sparkasse-csv";
    public TransactionSource Source => TransactionSource.CsvImport;

    public bool CanParse(string fileName, byte[] head)
    {
        var text = DecodeBytes(head);
        return text.Contains("Auftragskonto", StringComparison.OrdinalIgnoreCase) &&
               text.Contains(';', StringComparison.Ordinal);
    }

    public Result<BankStatementParseResult> Parse(Stream content)
    {
        string text;

        using (var memory = new MemoryStream())
        {
            content.CopyTo(memory);
            text = DecodeBytes(memory.ToArray());
        }

        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.TrimEnd('\r'))
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .ToList();

        if (lines.Count < 1)
            return Result.Failure<BankStatementParseResult>(
                FinanceErrors.ImportFailedWithMessage("CSV file is empty"));

        var header = SplitCsvLine(lines[0]);
        var columns = header
            .Select((name, i) => (name: name.Trim(), i))
            .ToDictionary(x => x.name, x => x.i, StringComparer.OrdinalIgnoreCase);

        var bookingDateIndex = FindColumn(columns, "Buchungstag");
        var amountIndex = FindColumn(columns, "Betrag");

        if (bookingDateIndex is null || amountIndex is null)
            return Result.Failure<BankStatementParseResult>(
                FinanceErrors.ImportFailedWithMessage("CSV header is missing Buchungstag or Betrag column"));

        var valueDateIndex = FindColumn(columns, "Valutadatum");
        var purposeIndex = FindColumn(columns, "Verwendungszweck");
        var counterpartyIndex = FindColumn(columns, "Beguenstigter/Zahlungspflichtiger", "Begünstigter/Zahlungspflichtiger");
        var ibanIndex = FindColumn(columns, "Kontonummer/IBAN", "IBAN");
        var referenceIndex = FindColumn(columns, "Kundenreferenz (End-to-End)", "Kundenreferenz");
        var currencyIndex = FindColumn(columns, "Waehrung", "Währung");

        var transactions = new List<ParsedTransaction>();
        var errors = new List<string>();

        for (var lineNumber = 1; lineNumber < lines.Count; lineNumber++)
        {
            var fields = SplitCsvLine(lines[lineNumber]);

            try
            {
                var bookingDate = ParseGermanDate(GetField(fields, bookingDateIndex));
                var amount = decimal.Parse(GetField(fields, amountIndex),
                    NumberStyles.Number, CultureInfo.GetCultureInfo("de-DE"));

                var reference = GetField(fields, referenceIndex);
                if (string.Equals(reference, "NOTPROVIDED", StringComparison.OrdinalIgnoreCase))
                    reference = null;

                transactions.Add(new ParsedTransaction(
                    bookingDate,
                    TryParseGermanDate(GetField(fields, valueDateIndex)),
                    amount,
                    EmptyToNull(GetField(fields, currencyIndex)),
                    EmptyToNull(GetField(fields, counterpartyIndex)),
                    EmptyToNull(GetField(fields, ibanIndex)),
                    EmptyToNull(GetField(fields, purposeIndex)),
                    EmptyToNull(reference)));
            }
            catch (Exception ex)
            {
                errors.Add($"Line {lineNumber + 1}: {ex.Message}");
            }
        }

        return Result.Success(new BankStatementParseResult(transactions, errors));
    }

    private static string DecodeBytes(byte[] bytes)
    {
        // UTF-8 BOM wins; otherwise try strict UTF-8 and fall back to Windows-1252 (typical bank export)
        if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            return Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

        try
        {
            return new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true)
                .GetString(bytes);
        }
        catch (DecoderFallbackException)
        {
            return Encoding.GetEncoding(1252).GetString(bytes);
        }
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

    private static DateOnly ParseGermanDate(string? value) =>
        TryParseGermanDate(value) ?? throw new FormatException($"Invalid date '{value}'");

    private static DateOnly? TryParseGermanDate(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        foreach (var format in DateFormats)
        {
            if (DateOnly.TryParseExact(value, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out var date))
                return date;
        }

        return null;
    }

    // Minimal quote-aware CSV splitter for semicolon separated lines
    private static List<string> SplitCsvLine(string line)
    {
        var fields = new List<string>();
        var current = new StringBuilder();
        var inQuotes = false;

        for (var i = 0; i < line.Length; i++)
        {
            var c = line[i];

            if (inQuotes)
            {
                if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                {
                    current.Append('"');
                    i++;
                }
                else if (c == '"')
                {
                    inQuotes = false;
                }
                else
                {
                    current.Append(c);
                }
            }
            else if (c == '"')
            {
                inQuotes = true;
            }
            else if (c == ';')
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        fields.Add(current.ToString());
        return fields;
    }
}
