using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.Library.Logger;
using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.Models.TodoDtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Todos.Commands;

public class UpdateTodoCommandHandler(
    ITodoCommands todoCommands,
    ILogger<UpdateTodoCommandHandler> logger) : IRequestHandler<UpdateTodoCommand, BaseResponse<GetToDoDto>>
{
    private readonly LoggerExtension<UpdateTodoCommandHandler> _logger = new(logger);
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<BaseResponse<GetToDoDto>> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetToDoDto>();
        try
        {
            response.Data = await _todoCommands.UpdateAsync(request, cancellationToken);
            if (response.Data is not null)
            {
                response.Succcess = true;
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
