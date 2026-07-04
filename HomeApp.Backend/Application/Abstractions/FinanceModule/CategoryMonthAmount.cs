namespace Application.Abstractions.FinanceModule;

public sealed record CategoryMonthAmount(int? CategoryId, int Month, decimal Sum, int Count);
