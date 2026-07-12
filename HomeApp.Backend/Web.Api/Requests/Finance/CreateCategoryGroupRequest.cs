using Application.Features.Finance.CategoryGroups.Commands;
using Domain.Entities.Finance.Enums;

namespace Web.Api.Requests.Finance;

public sealed record CreateCategoryGroupRequest
{
    public int HouseholdId { get; init; }
    public string Name { get; init; } = string.Empty;
    public CategoryType CategoryGroupType { get; init; }
    public decimal? TargetPercent { get; init; }

    public static explicit operator CreateCategoryGroupCommand(CreateCategoryGroupRequest request)
        => new(request.HouseholdId, request.Name, request.CategoryGroupType, request.TargetPercent);
}
