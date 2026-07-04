namespace Application.Features.Budgets.DTOs.Eva;

public sealed record EvaTotalsDto(
    decimal[] IncomeSoll,
    decimal[] IncomeIst,
    decimal[] ExpenseSoll,
    decimal[] ExpenseIst,
    decimal[] DiffSoll,
    decimal[] DiffIst,
    decimal YearIncomeSoll,
    decimal YearIncomeIst,
    decimal YearExpenseSoll,
    decimal YearExpenseIst,
    decimal YearDiffSoll,
    decimal YearDiffIst);
