using Application.Features.Finance.Transactions.Commands;

namespace Web.Api.Requests.Finance;

public sealed record UpdateTransactionRequest
{
    public int TransactionId { get; init; }
    public DateOnly BookingDate { get; init; }
    public DateOnly? ValueDate { get; init; }
    public decimal Amount { get; init; }
    public string? CounterpartyName { get; init; }
    public string? CounterpartyIban { get; init; }
    public string? Purpose { get; init; }

    public static explicit operator UpdateTransactionCommand(UpdateTransactionRequest request)
        => new(request.TransactionId, request.BookingDate, request.ValueDate, request.Amount,
            request.CounterpartyName, request.CounterpartyIban, request.Purpose);
}
