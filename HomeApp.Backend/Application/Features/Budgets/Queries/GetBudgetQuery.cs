using Application.Features.Budgets.DTOs;
using MediatR;
using SharedKernel;

namespace Application.Features.Budgets.Queries;

public sealed record GetBudgetQuery(int Year) : IRequest<Result<BudgetResponse>>;
