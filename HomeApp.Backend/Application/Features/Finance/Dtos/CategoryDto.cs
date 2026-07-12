using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;

namespace Application.Features.Finance.Dtos;

public sealed class CategoryDto
{
    public int CategoryId { get; set; }
    public int HouseholdId { get; set; }
    public string Name { get; set; } = default!;
    public CategoryType CategoryType { get; set; }
    public int? CategoryGroupId { get; set; }

    public static explicit operator CategoryDto(Category entity) =>
        new()
        {
            CategoryId = entity.CategoryId,
            HouseholdId = entity.HouseholdId,
            Name = entity.Name,
            CategoryType = entity.CategoryType,
            CategoryGroupId = entity.CategoryGroupId
        };
}
