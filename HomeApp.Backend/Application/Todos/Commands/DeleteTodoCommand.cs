using MediatR;
using SharedKernel;

namespace Application.Todos.Commands;

public record DeleteTodoCommand(int Id) : IRequest<BaseResponse<bool>>;
