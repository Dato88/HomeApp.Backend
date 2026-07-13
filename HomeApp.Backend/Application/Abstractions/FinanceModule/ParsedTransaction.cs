namespace Application.Abstractions.FinanceModule;

public sealed record ParsedTransaction(
    DateOnly BookingDate,
    DateOnly? ValueDate,
    decimal Amount,
    string? CurrencyCode,
    string? PaymentPartnerName,
    string? PaymentPartnerIban,
    string? Purpose,
    string? BankReference);
