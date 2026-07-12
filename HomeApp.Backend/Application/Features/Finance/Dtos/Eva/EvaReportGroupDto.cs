using Domain.Entities.Finance.Enums;

namespace Application.Features.Finance.Dtos.Eva;

public sealed record EvaReportGroupDto(
    int CategoryGroupId,
    int HouseholdId,
    string Name,
    CategoryType CategoryGroupType,
    decimal? TargetPercent,
    decimal? ActualPercentOfIncome,
    IReadOnlyList<EvaReportCategoryDto> Categories,
    decimal[] Ist,
    decimal YearIst);
