using Domain.Entities.Finance.Enums;

namespace Application.Features.Finance.Dtos.Eva;

// Ist: 12 months, index 0 = January. Grouped categories are displayed positive (flipped by the
// group type); ungrouped categories are flipped by their own type, Unknown stays signed.
public sealed record EvaReportCategoryDto(
    int CategoryId,
    int HouseholdId,
    string Name,
    CategoryType CategoryType,
    decimal[] Ist,
    decimal YearIst);
