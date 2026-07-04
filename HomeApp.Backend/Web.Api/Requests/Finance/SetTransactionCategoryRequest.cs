using Application.Features.Finance.Transactions.Commands;

namespace Web.Api.Requests.Finance;

public sealed record SetTransactionCategoryRequest
{
    public int TransactionId { get; init; }
    public int? CategoryId { get; init; }

    public static explicit operator SetTransactionCategoryCommand(SetTransactionCategoryRequest request)
        => new(request.TransactionId, request.CategoryId);
}
