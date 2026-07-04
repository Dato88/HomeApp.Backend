using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public static class Iban
{
    private static readonly Regex Shape = new("^[A-Z]{2}[0-9]{2}[A-Z0-9]{1,30}$", RegexOptions.Compiled);

    public static string Normalize(string value) =>
        string.Concat(value.Where(c => !char.IsWhiteSpace(c))).ToUpperInvariant();

    public static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var normalized = Normalize(value);

        if (!Shape.IsMatch(normalized))
            return false;

        // ISO 13616 mod-97 check: move the first four characters to the end,
        // replace letters with 10..35 and the remainder of the number mod 97 must be 1.
        var rearranged = normalized[4..] + normalized[..4];
        var remainder = 0;

        foreach (var c in rearranged)
        {
            var digits = char.IsLetter(c) ? (c - 'A' + 10).ToString() : c.ToString();

            foreach (var digit in digits)
                remainder = (remainder * 10 + (digit - '0')) % 97;
        }

        return remainder == 1;
    }
}
