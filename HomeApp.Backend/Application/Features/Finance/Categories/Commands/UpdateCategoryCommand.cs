using Domain.Entities.Finance.Enums;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Commands;

public sealed record UpdateCategoryCommand(
    int CategoryId,
    string Name,
    CategoryType CategoryType) : IRequest<Result<int>>;
