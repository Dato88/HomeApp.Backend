using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public record DeleteTodoCommand(int Id) : IRequest<BaseResponse<bool>>;
