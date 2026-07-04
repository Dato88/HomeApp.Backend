using Application.Features.Finance.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Categories.Queries;

public sealed record GetCategoriesQuery(int HouseholdId) : IRequest<Result<List<CategoryDto>>>;
