using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed record CreateHouseholdCommand(string Name) : IRequest<Result<int>>;
