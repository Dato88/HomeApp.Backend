using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed record UpdateHouseholdCommand(int HouseholdId, string Name) : IRequest<Result<int>>;
