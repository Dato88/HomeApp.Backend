namespace Application.Features.Finance.Dtos;

public sealed class TransactionListResponse
{
    public int TotalCount { get; set; }
    public IEnumerable<TransactionDto> Transactions { get; set; } = new List<TransactionDto>();
}
