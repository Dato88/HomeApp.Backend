using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Commands;

public sealed record CreateCategoryGroupCommand(
    int HouseholdId,
    string Name,
    CategoryType CategoryGroupType,
    decimal? TargetPercent) : IRequest<Result<int>>
{
    public static explicit operator CategoryGroup(CreateCategoryGroupCommand item) =>
        new()
        {
            HouseholdId = item.HouseholdId,
            Name = item.Name,
            CategoryGroupType = item.CategoryGroupType,
            TargetPercent = item.TargetPercent
        };
}
