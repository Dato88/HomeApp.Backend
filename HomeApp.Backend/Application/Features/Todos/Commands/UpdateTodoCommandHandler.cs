using Application.Abstractions.Logging;
using Application.Common.Interfaces.Todos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public class UpdateTodoCommandHandler(
    ITodoCommands todoCommands,
    IAppLogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, BaseResponse<bool>>
{
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<BaseResponse<bool>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        try
        {
            var isUpdated = await _todoCommands.UpdateAsync(request, cancellationToken);

            if (isUpdated)
            {
                response.Success = true;
                response.Message = "Update succeed!";
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;

            logger.LogError($"Update todo failed: {ex}");
        }

        return response;
    }
}
