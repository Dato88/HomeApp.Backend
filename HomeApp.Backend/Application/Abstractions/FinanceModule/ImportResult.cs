namespace Application.Abstractions.FinanceModule;

public sealed record ImportResult(int Imported, int SkippedDuplicates);
