using Domain.ValueObjects;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public sealed record DeleteTodoCommand(TodoId TodoId) : IRequest<Result>;
