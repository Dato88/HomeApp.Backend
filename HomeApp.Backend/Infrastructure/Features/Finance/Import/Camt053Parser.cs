using System.Globalization;
using System.Xml.Linq;
using Application.Abstractions.FinanceModule;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using SharedKernel;

namespace Infrastructure.Features.Finance.Import;

// ISO 20022 camt.053 (bank-to-customer statement). Namespace-agnostic so that
// camt.053.001.02 through .08 (and camt.052 intraday reports) all parse.
public sealed class Camt053Parser : IBankStatementParser
{
    public string FormatName => "camt053";
    public TransactionSource Source => TransactionSource.CamtImport;

    public bool CanParse(string fileName, byte[] head)
    {
        if (fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            return true;

        var text = System.Text.Encoding.UTF8.GetString(head);
        return text.Contains("<Document", StringComparison.OrdinalIgnoreCase) &&
               text.Contains("camt.05", StringComparison.OrdinalIgnoreCase);
    }

    public Result<BankStatementParseResult> Parse(Stream content)
    {
        XDocument document;

        try
        {
            document = XDocument.Load(content);
        }
        catch (Exception ex)
        {
            return Result.Failure<BankStatementParseResult>(
                FinanceErrors.ImportFailedWithMessage($"Invalid camt XML: {ex.Message}"));
        }

        var transactions = new List<ParsedTransaction>();
        var errors = new List<string>();
        var index = 0;

        foreach (var entry in document.Descendants().Where(e => e.Name.LocalName == "Ntry"))
        {
            index++;

            try
            {
                var amountElement = Child(entry, "Amt");
                if (amountElement is null)
                {
                    errors.Add($"Entry {index}: missing Amt");
                    continue;
                }

                var amount = decimal.Parse(amountElement.Value, CultureInfo.InvariantCulture);
                var currency = amountElement.Attribute("Ccy")?.Value;

                var creditDebit = Child(entry, "CdtDbtInd")?.Value;
                if (string.Equals(creditDebit, "DBIT", StringComparison.OrdinalIgnoreCase))
                    amount = -amount;

                var bookingDate = ParseDate(Child(entry, "BookgDt"));
                if (bookingDate is null)
                {
                    errors.Add($"Entry {index}: missing BookgDt");
                    continue;
                }

                var valueDate = ParseDate(Child(entry, "ValDt"));

                var reference = Child(entry, "NtryRef")?.Value;
                if (string.IsNullOrWhiteSpace(reference) ||
                    string.Equals(reference, "NOTPROVIDED", StringComparison.OrdinalIgnoreCase))
                    reference = Descendant(entry, "EndToEndId")?.Value;
                if (string.Equals(reference, "NOTPROVIDED", StringComparison.OrdinalIgnoreCase))
                    reference = null;

                // Partner: for debits the creditor, for credits the debtor
                var isDebit = amount < 0;
                var paymentPartnerName = Descendant(entry, isDebit ? "Cdtr" : "Dbtr") is { } party
                    ? Descendant(party, "Nm")?.Value
                    : null;
                var paymentPartnerIban = Descendant(entry, isDebit ? "CdtrAcct" : "DbtrAcct") is { } acct
                    ? Descendant(acct, "IBAN")?.Value
                    : null;

                var purpose = string.Join(" ",
                    entry.Descendants().Where(e => e.Name.LocalName == "Ustrd").Select(e => e.Value.Trim()));

                transactions.Add(new ParsedTransaction(
                    bookingDate.Value,
                    valueDate,
                    amount,
                    currency,
                    paymentPartnerName,
                    paymentPartnerIban,
                    string.IsNullOrWhiteSpace(purpose) ? null : purpose,
                    reference));
            }
            catch (Exception ex)
            {
                errors.Add($"Entry {index}: {ex.Message}");
            }
        }

        return Result.Success(new BankStatementParseResult(transactions, errors));
    }

    private static XElement? Child(XElement element, string localName) =>
        element.Elements().FirstOrDefault(e => e.Name.LocalName == localName);

    private static XElement? Descendant(XElement element, string localName) =>
        element.Descendants().FirstOrDefault(e => e.Name.LocalName == localName);

    private static DateOnly? ParseDate(XElement? dateContainer)
    {
        if (dateContainer is null)
            return null;

        var value = Child(dateContainer, "Dt")?.Value ?? Child(dateContainer, "DtTm")?.Value;

        if (string.IsNullOrWhiteSpace(value))
            return null;

        return DateOnly.FromDateTime(DateTime.Parse(value, CultureInfo.InvariantCulture));
    }
}
