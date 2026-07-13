using Domain.Entities.Finance;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Transactions.Commands;

public sealed record UpdateTransactionCommand(
    int TransactionId,
    DateOnly BookingDate,
    DateOnly? ValueDate,
    decimal Amount,
    string? PaymentPartnerName,
    string? PaymentPartnerIban,
    string? Purpose) : IRequest<Result<int>>
{
    public static explicit operator Transaction(UpdateTransactionCommand item) =>
        new()
        {
            TransactionId = item.TransactionId,
            BookingDate = item.BookingDate,
            ValueDate = item.ValueDate,
            Amount = item.Amount,
            PaymentPartnerName = item.PaymentPartnerName,
            PaymentPartnerIban = string.IsNullOrWhiteSpace(item.PaymentPartnerIban)
                ? null
                : Domain.ValueObjects.Iban.Normalize(item.PaymentPartnerIban),
            Purpose = item.Purpose
        };
}
