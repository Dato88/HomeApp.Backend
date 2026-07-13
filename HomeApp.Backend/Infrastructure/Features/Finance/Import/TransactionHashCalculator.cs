using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Application.Abstractions.FinanceModule;

namespace Infrastructure.Features.Finance.Import;

// One canonical dedup scheme for all import formats, so a CSV and a CAMT export of the same account
// dedupe against each other. BankReference is deliberately NOT part of the hash (format dependent).
internal static class TransactionHashCalculator
{
    private static readonly Regex Whitespace = new(@"\s+", RegexOptions.Compiled);

    public static string ComputeKey(int accountId, ParsedTransaction transaction)
    {
        var iban = transaction.PaymentPartnerIban is null
            ? string.Empty
            : Domain.ValueObjects.Iban.Normalize(transaction.PaymentPartnerIban);

        var purpose = transaction.Purpose is null
            ? string.Empty
            : Whitespace.Replace(transaction.Purpose, " ").Trim().ToUpperInvariant();

        return string.Join('|',
            accountId.ToString(CultureInfo.InvariantCulture),
            transaction.BookingDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            transaction.Amount.ToString("0.00", CultureInfo.InvariantCulture),
            iban,
            purpose);
    }

    // The occurrence suffix distinguishes identical bookings within one file (e.g. two equal debits
    // on the same day); across two different files such bookings still collide - accepted limitation.
    public static string ComputeHash(string key, int occurrence)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes($"{key}|{occurrence}"));
        return Convert.ToHexStringLower(bytes);
    }
}
