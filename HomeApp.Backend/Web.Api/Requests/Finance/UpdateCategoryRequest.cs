using Application.Features.Finance.Categories.Commands;
using Domain.Entities.Finance.Enums;

namespace Web.Api.Requests.Finance;

public sealed record UpdateCategoryRequest
{
    public int CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public CategoryType CategoryType { get; init; }

    public static explicit operator UpdateCategoryCommand(UpdateCategoryRequest request)
        => new(request.CategoryId, request.Name, request.CategoryType);
}
