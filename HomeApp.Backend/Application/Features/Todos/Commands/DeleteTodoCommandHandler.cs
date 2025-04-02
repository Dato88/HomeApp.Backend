using Application.Abstractions.Logging;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

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

            logger.LogError($"Delete todo failed: {ex}");
        }

        return response;
    }
}
