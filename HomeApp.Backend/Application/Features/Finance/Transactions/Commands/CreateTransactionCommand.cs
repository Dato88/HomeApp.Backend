using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed record CreateTransactionCommand(
    int AccountId,
    DateOnly BookingDate,
    DateOnly? ValueDate,
    decimal Amount,
    string? CounterpartyName,
    string? CounterpartyIban,
    string? Purpose,
    int? CategoryId) : IRequest<Result<int>>
{
    public static explicit operator Transaction(CreateTransactionCommand item) =>
        new()
        {
            AccountId = item.AccountId,
            BookingDate = item.BookingDate,
            ValueDate = item.ValueDate,
            Amount = item.Amount,
            CounterpartyName = item.CounterpartyName,
            CounterpartyIban = string.IsNullOrWhiteSpace(item.CounterpartyIban)
                ? null
                : Domain.ValueObjects.Iban.Normalize(item.CounterpartyIban),
            Purpose = item.Purpose,
            CategoryId = item.CategoryId,
            Source = TransactionSource.Manual
        };
}
