using Domain.Entities.Finance;

namespace Application.Abstractions.FinanceModule;

// Read model for the E+A report: groups and categories of the selected households plus the
// signed monthly transaction totals per category (CategoryId null = uncategorized)
public sealed record EvaReportData(
    IReadOnlyList<CategoryGroup> Groups,
    IReadOnlyList<Category> Categories,
    IReadOnlyList<CategoryMonthAmount> Totals);
