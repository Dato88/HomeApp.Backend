using Application.Features.Finance.Categories.Commands;
using Domain.Entities.Finance.Enums;

namespace Web.Api.Requests.Finance;

public sealed record CreateCategoryRequest
{
    public int HouseholdId { get; init; }
    public string Name { get; init; } = string.Empty;
    public CategoryType CategoryType { get; init; }
    public int? CategoryGroupId { get; init; }

    public static explicit operator CreateCategoryCommand(CreateCategoryRequest request)
        => new(request.HouseholdId, request.Name, request.CategoryType, request.CategoryGroupId);
}
