using Application.Features.Todos.Dtos;
using MediatR;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Application.Features.Todos.Queries;

public sealed record GetTodoByIdQuery(TodoId TodoId) : IRequest<Result<GetToDoResponse>>;
