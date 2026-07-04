using Domain.Entities.Finance;
using Domain.Entities.Finance.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Commands;

public sealed record CreateCategoryCommand(
    int HouseholdId,
    string Name,
    CategoryType CategoryType) : IRequest<Result<int>>
{
    public static explicit operator Category(CreateCategoryCommand item) =>
        new() { HouseholdId = item.HouseholdId, Name = item.Name, CategoryType = item.CategoryType };
}
