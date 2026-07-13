using Application.Features.Finance.Transactions.Commands;

namespace Web.Api.Requests.Finance;

public sealed record CreateTransactionRequest
{
    public int AccountId { get; init; }
    public DateOnly BookingDate { get; init; }
    public DateOnly? ValueDate { get; init; }
    public decimal Amount { get; init; }
    public string? PaymentPartnerName { get; init; }
    public string? PaymentPartnerIban { get; init; }
    public string? Purpose { get; init; }
    public int? CategoryId { get; init; }

    public static explicit operator CreateTransactionCommand(CreateTransactionRequest request)
        => new(request.AccountId, request.BookingDate, request.ValueDate, request.Amount,
            request.PaymentPartnerName, request.PaymentPartnerIban, request.Purpose, request.CategoryId);
}
