using Application.Features.Todos.Dtos;
using Domain.ValueObjects;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public sealed record GetTodoByIdQuery(TodoId TodoId) : IRequest<Result<GetToDoResponse>>;
