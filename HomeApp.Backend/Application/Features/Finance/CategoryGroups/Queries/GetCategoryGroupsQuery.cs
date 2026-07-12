using Application.Features.Finance.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.CategoryGroups.Queries;

public sealed record GetCategoryGroupsQuery(int HouseholdId) : IRequest<Result<List<CategoryGroupDto>>>;
