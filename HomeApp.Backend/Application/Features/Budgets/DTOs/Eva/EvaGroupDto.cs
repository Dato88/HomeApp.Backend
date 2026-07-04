using Domain.Entities.Budgets.Enums;

namespace Application.Features.Budgets.DTOs.Eva;

public sealed record EvaGroupDto(
    int BudgetGroupId,
    int Index,
    string Title,
    BudgetGroupType BudgetGroupType,
    decimal? TargetPercent,
    decimal? PlannedPercentOfIncome,
    decimal? ActualPercentOfIncome,
    IReadOnlyList<EvaRowDto> Rows,
    decimal[] Soll,
    decimal[] Ist,
    decimal[] Diff,
    decimal YearSoll,
    decimal YearIst);
