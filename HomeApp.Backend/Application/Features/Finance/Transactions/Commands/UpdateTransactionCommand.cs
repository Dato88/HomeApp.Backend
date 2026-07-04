using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed record UpdateTransactionCommand(
    int TransactionId,
    DateOnly BookingDate,
    DateOnly? ValueDate,
    decimal Amount,
    string? CounterpartyName,
    string? CounterpartyIban,
    string? Purpose) : IRequest<Result<int>>
{
    public static explicit operator Transaction(UpdateTransactionCommand item) =>
        new()
        {
            TransactionId = item.TransactionId,
            BookingDate = item.BookingDate,
            ValueDate = item.ValueDate,
            Amount = item.Amount,
            CounterpartyName = item.CounterpartyName,
            CounterpartyIban = string.IsNullOrWhiteSpace(item.CounterpartyIban)
                ? null
                : Domain.ValueObjects.Iban.Normalize(item.CounterpartyIban),
            Purpose = item.Purpose
        };
}
