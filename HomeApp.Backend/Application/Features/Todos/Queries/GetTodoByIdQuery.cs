using Application.Features.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public sealed record GetTodoByIdQuery(int TodoId) : IRequest<Result<GetToDoResponse>>;
