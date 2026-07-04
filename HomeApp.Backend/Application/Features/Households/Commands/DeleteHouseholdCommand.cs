using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed record DeleteHouseholdCommand(int HouseholdId) : IRequest<Result<int>>;
