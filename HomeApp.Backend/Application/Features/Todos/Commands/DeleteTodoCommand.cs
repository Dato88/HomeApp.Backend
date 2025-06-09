using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public sealed record DeleteTodoCommand(int Id) : IRequest<Result>;
