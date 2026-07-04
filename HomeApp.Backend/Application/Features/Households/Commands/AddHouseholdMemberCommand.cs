using MediatR;
using SharedKernel;

namespace Application.Features.Households.Commands;

public sealed record AddHouseholdMemberCommand(int HouseholdId, string? Email, int? PersonId)
    : IRequest<Result<int>>;
