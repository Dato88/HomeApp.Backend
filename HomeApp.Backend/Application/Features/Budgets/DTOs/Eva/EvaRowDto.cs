namespace Application.Features.Budgets.DTOs.Eva;

// All month arrays have a fixed length of 12; index 0 = January
public sealed record EvaRowDto(
    int BudgetRowId,
    int Index,
    string Title,
    int? CategoryId,
    string? CategoryName,
    decimal[] Soll,
    decimal[] Ist,
    decimal[] Diff,
    decimal YearSoll,
    decimal YearIst);
