using Application.Features.Households.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Households.Queries;

public sealed record GetHouseholdsQuery : IRequest<Result<List<HouseholdResponse>>>;
