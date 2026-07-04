namespace Application.Features.Finance.Dtos;

public sealed record ImportTransactionsResponse(
    int Imported,
    int SkippedDuplicates,
    int Failed,
    IReadOnlyList<string> Errors);
