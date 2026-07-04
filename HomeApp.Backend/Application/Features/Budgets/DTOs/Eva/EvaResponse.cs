namespace Application.Features.Budgets.DTOs.Eva;

public sealed record EvaResponse(
    int BudgetId,
    int HouseholdId,
    int Year,
    IReadOnlyList<EvaGroupDto> Groups,
    EvaTotalsDto Totals,
    decimal[] UnassignedIst,
    int UnassignedTransactionCount);
