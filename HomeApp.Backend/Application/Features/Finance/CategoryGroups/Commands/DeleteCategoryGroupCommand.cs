using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Commands;

public sealed record DeleteCategoryGroupCommand(int CategoryGroupId) : IRequest<Result<int>>;
