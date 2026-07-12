namespace Application.Features.Finance.Dtos.Eva;

// UnassignedIst (signed) collects everything that is not visible in the groups/categories of the
// selected households: uncategorized transactions and transactions categorized in a household that
// was not requested — nothing disappears from the report.
public sealed record EvaReportResponse(
    int Year,
    IReadOnlyList<int> HouseholdIds,
    IReadOnlyList<EvaReportGroupDto> Groups,
    IReadOnlyList<EvaReportCategoryDto> UngroupedCategories,
    EvaReportTotalsDto Totals,
    decimal[] UnassignedIst,
    int UnassignedTransactionCount);
