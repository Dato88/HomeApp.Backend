using Application.Features.Finance.CategoryGroups.Commands;
using Domain.Entities.Finance.Enums;

namespace Web.Api.Requests.Finance;

public sealed record UpdateCategoryGroupRequest
{
    public int CategoryGroupId { get; init; }
    public string Name { get; init; } = string.Empty;
    public CategoryType CategoryGroupType { get; init; }
    public decimal? TargetPercent { get; init; }

    public static explicit operator UpdateCategoryGroupCommand(UpdateCategoryGroupRequest request)
        => new(request.CategoryGroupId, request.Name, request.CategoryGroupType, request.TargetPercent);
}
