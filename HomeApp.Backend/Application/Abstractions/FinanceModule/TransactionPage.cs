using Domain.Entities.Finance;

namespace Application.Abstractions.FinanceModule;

public sealed record TransactionPage(int TotalCount, List<Transaction> Items);
