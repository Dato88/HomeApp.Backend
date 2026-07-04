namespace Application.Abstractions.FinanceModule;

public sealed record ParsedTransaction(
    DateOnly BookingDate,
    DateOnly? ValueDate,
    decimal Amount,
    string? CurrencyCode,
    string? CounterpartyName,
    string? CounterpartyIban,
    string? Purpose,
    string? BankReference);
