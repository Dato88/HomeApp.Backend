using Application.Common.Interfaces.Todos;
using Infrastructure.Logger;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Todos.Commands;

public class DeleteTodoCommandHandler(
    ITodoCommands todoCommands,
    ILogger<DeleteTodoCommandHandler> logger) : IRequestHandler<DeleteTodoCommand, BaseResponse<bool>>
{
    private readonly LoggerExtension<DeleteTodoCommandHandler> _logger = new(logger);
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

            _logger.LogException($"Delete todo failed: {ex}", DateTime.Now);
        }

        return response;
    }
}
