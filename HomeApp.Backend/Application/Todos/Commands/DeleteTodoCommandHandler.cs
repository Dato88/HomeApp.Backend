using Application.Abstractions.Logging;
using Application.Common.Interfaces.Todos;
using MediatR;
using SharedKernel;

namespace Application.Todos.Commands;

public class DeleteTodoCommandHandler(
    ITodoCommands todoCommands,
    IAppLogger<DeleteTodoCommandHandler> logger) : IRequestHandler<DeleteTodoCommand, BaseResponse<bool>>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<BaseResponse<bool>> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var isDeleted = await _todoCommands.DeleteAsync(request.Id, cancellationToken);

            if (isDeleted)
            {
                response.Success = true;
                response.Message = "Delete succeed!";
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;

            logger.LogCritical($"Delete todo failed: {ex}");
        }

        return response;
    }
}
