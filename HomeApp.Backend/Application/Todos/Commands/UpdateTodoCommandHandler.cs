using Application.Common.Interfaces.Todos;
using Infrastructure.Logger;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Todos.Commands;

public class UpdateTodoCommandHandler(
    ITodoCommands todoCommands,
    ILogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, BaseResponse<bool>>
{
    private readonly LoggerExtension<UpdateTodoCommandHandler> _logger = new(logger);
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

            _logger.LogException($"Update todo failed: {ex}", DateTime.Now);
        }

        return response;
    }
}
