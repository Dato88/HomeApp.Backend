using Application.Features.Budgets.DTOs.Eva;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Queries;

public sealed record GetEvaQuery(int HouseholdId, int Year) : IRequest<Result<EvaResponse>>;
