using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Commands;

public sealed record DeleteCategoryCommand(int CategoryId) : IRequest<Result<int>>;
