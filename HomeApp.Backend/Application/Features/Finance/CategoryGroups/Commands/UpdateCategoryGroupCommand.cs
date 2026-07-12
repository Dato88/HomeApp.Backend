using Domain.Entities.Finance.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Commands;

public sealed record UpdateCategoryGroupCommand(
    int CategoryGroupId,
    string Name,
    CategoryType CategoryGroupType,
    decimal? TargetPercent) : IRequest<Result<int>>;
