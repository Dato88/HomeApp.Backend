using System.Globalization;
using System.Text.RegularExpressions;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Infrastructure.Features.Finance.Import;

// Deutsche Bank private "Kontoauszug" PDF (columns Buchung/Valuta/Vorgang/Soll/Haben). PDFs carry
// no data structure, only positioned text, so parsing is coordinate-based: the header words define
// the column x-ranges and rows are reconstructed from word baselines. Best effort by design —
// entries a layout change breaks end up in the error list instead of the import.
public sealed class DeutscheBankPdfParser : IBankStatementParser
{
    private const double LineTolerance = 2.5;
    private const double ColumnTolerance = 2.0;

    private static readonly Regex DayRegex = new(@"^\d{2}\.\d{2}\.$", RegexOptions.Compiled);
    private static readonly Regex YearRegex = new(@"^\d{4}$", RegexOptions.Compiled);
    private static readonly Regex AmountRegex = new(@"^[+-]?\d{1,3}(\.\d{3})*,\d{2}$", RegexOptions.Compiled);

    // Vorgang lines that are SEPA metadata, not part of the purpose (both umlaut spellings —
    // text extraction does not always yield composed characters)
    private static readonly string[] MetadataPrefixes =
        ["Gläubiger-ID", "Glaeubiger-ID", "Mand-ID", "ABWA", "BIC", "RCUR", "OTHR", "FRST", "OOFF"];

    public string FormatName => "deutsche-bank-pdf";
    public TransactionSource Source => TransactionSource.PdfImport;

    public bool CanParse(string fileName, byte[] head) =>
        fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) ||
        (head.Length >= 4 && head.AsSpan(0, 4).SequenceEqual("%PDF"u8));

    public Result<BankStatementParseResult> Parse(Stream content)
    {
        try
        {
            using var document = PdfDocument.Open(content);

            var transactions = new List<ParsedTransaction>();
            var errors = new List<string>();
            var headerFound = false;
            var stopped = false;

            foreach (var page in document.GetPages())
            {
                if (stopped)
                    break;

                headerFound |= ParsePage(page, transactions, errors, ref stopped);
            }

            if (!headerFound)
                return Result.Failure<BankStatementParseResult>(FinanceErrors.ImportFailedWithMessage(
                    "No statement table (Buchung/Valuta/Vorgang/Soll/Haben) found in PDF"));

            return Result.Success(new BankStatementParseResult(transactions, errors));
        }
        catch (Exception ex)
        {
            return Result.Failure<BankStatementParseResult>(
                FinanceErrors.ImportFailedWithMessage($"File is not a readable PDF: {ex.Message}"));
        }
    }

    private static bool ParsePage(Page page, List<ParsedTransaction> transactions, List<string> errors,
        ref bool stopped)
    {
        var lines = GroupIntoLines(page.GetWords());

        var headerIndex = FindHeader(lines, out var buchungX, out var valutaX, out var vorgangX,
            out var sollRight, out var habenRight);

        if (headerIndex < 0)
            return false;

        PendingEntry? current = null;

        for (var i = headerIndex + 1; i < lines.Count; i++)
        {
            var line = lines[i];
            var fullText = string.Join(' ', line.Words.Select(w => w.Text));

            if (fullText.Contains("Neuer Saldo", StringComparison.Ordinal) ||
                fullText.Contains("Wichtige Hinweise", StringComparison.Ordinal))
            {
                stopped = true;
                break;
            }

            var buchungText = ColumnText(line, buchungX, valutaX);
            var valutaText = ColumnText(line, valutaX, vorgangX);

            if (DayRegex.IsMatch(buchungText))
            {
                Finalize(current, transactions, errors);

                current = new PendingEntry
                {
                    BookingDay = buchungText,
                    ValutaDay = DayRegex.IsMatch(valutaText) ? valutaText : null
                };

                var (vorgangText, amount) = SplitVorgangAndAmount(line, vorgangX, sollRight, habenRight);
                current.Amount = amount;
                AddVorgangLine(current, vorgangText);
                continue;
            }

            if (current is null)
                continue;

            // The year sits on the line below the day (layout: "02.03." / "2026" stacked)
            if (current.Year is null && YearRegex.IsMatch(buchungText))
            {
                current.Year = int.Parse(buchungText, CultureInfo.InvariantCulture);
                current.ValutaYear = YearRegex.IsMatch(valutaText)
                    ? int.Parse(valutaText, CultureInfo.InvariantCulture)
                    : null;
            }

            AddVorgangLine(current, ColumnText(line, vorgangX, double.MaxValue));
        }

        Finalize(current, transactions, errors);
        return true;
    }

    private static int FindHeader(List<TextLine> lines, out double buchungX, out double valutaX,
        out double vorgangX, out double sollRight, out double habenRight)
    {
        buchungX = valutaX = vorgangX = sollRight = habenRight = 0;

        for (var i = 0; i < lines.Count; i++)
        {
            var words = lines[i].Words;
            var buchung = words.FirstOrDefault(w => w.Text == "Buchung");
            var valuta = words.FirstOrDefault(w => w.Text == "Valuta");
            var vorgang = words.FirstOrDefault(w => w.Text == "Vorgang");
            var soll = words.FirstOrDefault(w => w.Text == "Soll");
            var haben = words.FirstOrDefault(w => w.Text == "Haben");

            if (buchung is null || valuta is null || vorgang is null || soll is null || haben is null)
                continue;

            buchungX = buchung.BoundingBox.Left;
            valutaX = valuta.BoundingBox.Left;
            vorgangX = vorgang.BoundingBox.Left;
            sollRight = soll.BoundingBox.Right;
            habenRight = haben.BoundingBox.Right;
            return i;
        }

        return -1;
    }

    // Splits an entry-start line into the Vorgang text and the (right-aligned) Soll/Haben amount
    private static (string VorgangText, decimal? Amount) SplitVorgangAndAmount(TextLine line,
        double vorgangX, double sollRight, double habenRight)
    {
        var words = line.Words
            .Where(w => w.BoundingBox.Left >= vorgangX - ColumnTolerance)
            .ToList();

        decimal? amount = null;

        if (words.Count > 0 && AmountRegex.IsMatch(words[^1].Text))
        {
            var valueWord = words[^1];
            var value = decimal.Parse(valueWord.Text, NumberStyles.Number,
                CultureInfo.GetCultureInfo("de-DE"));
            words.RemoveAt(words.Count - 1);

            bool negative;

            if (valueWord.Text.StartsWith('-'))
            {
                negative = true;
            }
            else if (words.Count > 0 && words[^1].Text is "-" or "+")
            {
                negative = words[^1].Text == "-";
                words.RemoveAt(words.Count - 1);
            }
            else
            {
                // No printed sign: the right-aligned column decides (Soll = debit)
                negative = Math.Abs(valueWord.BoundingBox.Right - sollRight) <=
                           Math.Abs(valueWord.BoundingBox.Right - habenRight);
            }

            amount = negative ? -Math.Abs(value) : Math.Abs(value);
        }

        return (string.Join(' ', words.Select(w => w.Text)), amount);
    }

    private static void AddVorgangLine(PendingEntry entry, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        if (text.StartsWith("Verwendungszweck", StringComparison.OrdinalIgnoreCase))
        {
            entry.MarkerSeen = true;
            return;
        }

        if (text.StartsWith("IBAN ", StringComparison.Ordinal))
        {
            entry.Iban ??= text[5..].Replace(" ", "", StringComparison.Ordinal);
            return;
        }

        if (MetadataPrefixes.Any(prefix => text.StartsWith(prefix, StringComparison.Ordinal)))
            return;

        if (entry.MarkerSeen)
            entry.Purpose.Add(text);
        else
            entry.PreMarker.Add(text);
    }

    private static void Finalize(PendingEntry? entry, List<ParsedTransaction> transactions,
        List<string> errors)
    {
        if (entry is null)
            return;

        try
        {
            if (entry.Year is null)
                throw new FormatException("Missing year line below the booking date");

            if (entry.Amount is null)
                throw new FormatException("Missing Soll/Haben amount");

            var bookingDate = DateOnly.ParseExact($"{entry.BookingDay}{entry.Year}", "dd.MM.yyyy",
                CultureInfo.InvariantCulture);

            DateOnly? valueDate = entry.ValutaDay is not null && entry.ValutaYear is not null
                ? DateOnly.ParseExact($"{entry.ValutaDay}{entry.ValutaYear}", "dd.MM.yyyy",
                    CultureInfo.InvariantCulture)
                : null;

            // PreMarker[0] is the transaction type ("SEPA Lastschrifteinzug von"), the payee follows
            var counterparty = entry.PreMarker.Count > 1 ? entry.PreMarker[1] : null;
            var purpose = entry.Purpose.Count > 0 ? string.Join(' ', entry.Purpose) : null;

            transactions.Add(new ParsedTransaction(
                bookingDate,
                valueDate,
                entry.Amount.Value,
                null,
                counterparty,
                entry.Iban,
                purpose,
                null));
        }
        catch (Exception ex)
        {
            errors.Add($"Entry {entry.BookingDay}{entry.Year}: {ex.Message}");
        }
    }

    private static List<TextLine> GroupIntoLines(IEnumerable<Word> words)
    {
        var lines = new List<TextLine>();

        foreach (var word in words.OrderByDescending(w => w.BoundingBox.Bottom))
        {
            var line = lines.FirstOrDefault(l => Math.Abs(l.Y - word.BoundingBox.Bottom) <= LineTolerance);

            if (line is null)
            {
                line = new TextLine(word.BoundingBox.Bottom);
                lines.Add(line);
            }

            line.Words.Add(word);
        }

        foreach (var line in lines)
            line.Words.Sort((a, b) => a.BoundingBox.Left.CompareTo(b.BoundingBox.Left));

        return lines;
    }

    private static string ColumnText(TextLine line, double fromX, double toX) =>
        string.Join(' ', line.Words
            .Where(w => w.BoundingBox.Left >= fromX - ColumnTolerance &&
                        w.BoundingBox.Left < toX - ColumnTolerance)
            .Select(w => w.Text));

    private sealed class TextLine(double y)
    {
        public double Y { get; } = y;
        public List<Word> Words { get; } = [];
    }

    private sealed class PendingEntry
    {
        public string? BookingDay { get; init; }
        public string? ValutaDay { get; init; }
        public int? Year { get; set; }
        public int? ValutaYear { get; set; }
        public decimal? Amount { get; set; }
        public string? Iban { get; set; }
        public bool MarkerSeen { get; set; }
        public List<string> PreMarker { get; } = [];
        public List<string> Purpose { get; } = [];
    }
}
