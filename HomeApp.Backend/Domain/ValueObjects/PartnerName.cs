namespace Domain.ValueObjects;

public static class PartnerName
{
    // Matching key for payment partner names: trim, collapse inner whitespace,
    // invariant uppercase. Deliberately no fuzzy matching.
    public static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : string.Join(' ', value.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries))
                .ToUpperInvariant();
}
