using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;

namespace Application.Features.Finance.Dtos;

public sealed class CategoryGroupDto
{
    public int CategoryGroupId { get; set; }
    public int HouseholdId { get; set; }
    public string Name { get; set; } = default!;
    public CategoryType CategoryGroupType { get; set; }
    public decimal? TargetPercent { get; set; }

    public static explicit operator CategoryGroupDto(CategoryGroup entity) =>
        new()
        {
            CategoryGroupId = entity.CategoryGroupId,
            HouseholdId = entity.HouseholdId,
            Name = entity.Name,
            CategoryGroupType = entity.CategoryGroupType,
            TargetPercent = entity.TargetPercent
        };
}
