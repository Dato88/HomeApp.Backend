namespace Application.Features.Finance.Dtos.Eva;

public sealed record EvaReportTotalsDto(
    decimal[] IncomeIst,
    decimal[] ExpenseIst,
    decimal[] DiffIst,
    decimal YearIncomeIst,
    decimal YearExpenseIst,
    decimal YearDiffIst);
