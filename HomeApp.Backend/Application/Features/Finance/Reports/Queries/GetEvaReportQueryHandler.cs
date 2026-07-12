using Application.Abstractions.FinanceModule;
using Application.Abstractions.Logging;
using Application.Features.Finance.Dtos.Eva;
using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Reports.Queries;

public sealed class GetEvaReportQueryHandler(
    IReportQueries reportQueries,
    IAppLogger<GetEvaReportQueryHandler> logger)
    : IRequestHandler<GetEvaReportQuery, Result<EvaReportResponse>>
{
    private readonly IReportQueries _reportQueries = reportQueries;
    private readonly IAppLogger<GetEvaReportQueryHandler> _logger = logger;

    public async Task<Result<EvaReportResponse>> Handle(GetEvaReportQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var householdIds = request.HouseholdIds.Distinct().ToList();

            var dataResult = await _reportQueries.GetEvaReportDataAsync(householdIds, request.Year,
                cancellationToken);

            if (dataResult.IsFailure)
                return Result.Failure<EvaReportResponse>(dataResult.Error);

            var response = BuildResponse(householdIds, request.Year, dataResult.Value);

            return Result.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Get eva report failed: {ex}");

            return Result.Failure<EvaReportResponse>(FinanceErrors.UnexpectedError(ex.Message));
        }
    }

    private static EvaReportResponse BuildResponse(IReadOnlyList<int> householdIds, int year, EvaReportData data)
    {
        // Raw (signed) IST per category and month
        var istByCategory = new Dictionary<int, decimal[]>();

        foreach (var total in data.Totals.Where(t => t.CategoryId.HasValue))
        {
            if (!istByCategory.TryGetValue(total.CategoryId!.Value, out var amounts))
                istByCategory[total.CategoryId.Value] = amounts = new decimal[12];

            amounts[total.Month - 1] += total.Sum;
        }

        var incomeIst = new decimal[12];
        var expenseIst = new decimal[12];

        // Grouped categories: displayed positive for both group types (like the Excel sheet)
        var groups = new List<EvaReportGroupDto>();
        var groupBuffer = new List<(CategoryGroup Group, List<EvaReportCategoryDto> Categories, decimal[] Ist)>();

        var orderedGroups = data.Groups
            .OrderBy(g => g.CategoryGroupType == CategoryType.Income ? 0 : 1)
            .ThenBy(g => g.Name, StringComparer.OrdinalIgnoreCase);

        foreach (var group in orderedGroups)
        {
            var groupIst = new decimal[12];
            var categories = new List<EvaReportCategoryDto>();

            foreach (var category in data.Categories.Where(c => c.CategoryGroupId == group.CategoryGroupId))
            {
                var ist = new decimal[12];

                if (istByCategory.TryGetValue(category.CategoryId, out var raw))
                {
                    for (var month = 0; month < 12; month++)
                        ist[month] = group.CategoryGroupType == CategoryType.Expense
                            ? -raw[month]
                            : raw[month];
                }

                for (var month = 0; month < 12; month++)
                    groupIst[month] += ist[month];

                categories.Add(new EvaReportCategoryDto(category.CategoryId, category.HouseholdId,
                    category.Name, category.CategoryType, ist, ist.Sum()));
            }

            for (var month = 0; month < 12; month++)
            {
                if (group.CategoryGroupType == CategoryType.Income)
                    incomeIst[month] += groupIst[month];
                else
                    expenseIst[month] += groupIst[month];
            }

            groupBuffer.Add((group, categories, groupIst));
        }

        // Ungrouped categories: flipped by their own type; Unknown stays signed and is classified
        // into the totals per month by sign so every categorized booking lands in the totals
        var ungrouped = new List<EvaReportCategoryDto>();

        foreach (var category in data.Categories.Where(c => c.CategoryGroupId == null))
        {
            var ist = new decimal[12];

            if (istByCategory.TryGetValue(category.CategoryId, out var raw))
            {
                for (var month = 0; month < 12; month++)
                {
                    ist[month] = category.CategoryType == CategoryType.Expense ? -raw[month] : raw[month];

                    switch (category.CategoryType)
                    {
                        case CategoryType.Income:
                            incomeIst[month] += raw[month];
                            break;
                        case CategoryType.Expense:
                            expenseIst[month] += -raw[month];
                            break;
                        default:
                            if (raw[month] >= 0)
                                incomeIst[month] += raw[month];
                            else
                                expenseIst[month] += -raw[month];
                            break;
                    }
                }
            }

            ungrouped.Add(new EvaReportCategoryDto(category.CategoryId, category.HouseholdId,
                category.Name, category.CategoryType, ist, ist.Sum()));
        }

        var yearIncomeIst = incomeIst.Sum();

        foreach (var (group, categories, groupIst) in groupBuffer)
            groups.Add(new EvaReportGroupDto(group.CategoryGroupId, group.HouseholdId, group.Name,
                group.CategoryGroupType, group.TargetPercent,
                Percent(groupIst.Sum(), yearIncomeIst),
                categories, groupIst, groupIst.Sum()));

        var diffIst = new decimal[12];

        for (var month = 0; month < 12; month++)
            diffIst[month] = incomeIst[month] - expenseIst[month];

        var totalsDto = new EvaReportTotalsDto(incomeIst, expenseIst, diffIst,
            yearIncomeIst, expenseIst.Sum(), yearIncomeIst - expenseIst.Sum());

        // Everything without a visible category (uncategorized, or categorized in a household that
        // was not requested), signed
        var knownCategoryIds = data.Categories.Select(c => c.CategoryId).ToHashSet();
        var unassignedIst = new decimal[12];
        var unassignedCount = 0;

        foreach (var total in data.Totals)
        {
            if (total.CategoryId.HasValue && knownCategoryIds.Contains(total.CategoryId.Value))
                continue;

            unassignedIst[total.Month - 1] += total.Sum;
            unassignedCount += total.Count;
        }

        return new EvaReportResponse(year, householdIds, groups, ungrouped, totalsDto, unassignedIst,
            unassignedCount);
    }

    // Negative base (e.g. a year dominated by refunds) would produce nonsense percentages
    private static decimal? Percent(decimal value, decimal baseValue) =>
        baseValue <= 0 ? null : Math.Round(value / baseValue * 100, 2, MidpointRounding.AwayFromZero);
}
