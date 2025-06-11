using MediatR;
using SharedKernel;
using SharedKernel.ValueObjects;

namespace Application.Features.Todos.Commands;

public sealed record DeleteTodoCommand(TodoId TodoId) : IRequest<Result>;
