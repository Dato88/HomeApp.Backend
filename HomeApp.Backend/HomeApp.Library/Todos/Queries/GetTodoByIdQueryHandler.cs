using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.Library.Logger;
using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.Todos.Dtos;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Todos.Queries;

public class GetTodoByIdQueryHandler(
    ITodoQueries todoQueries,
    ILogger<GetTodoByIdQueryHandler> logger)
    : IRequestHandler<GetTodoByIdQuery, BaseResponse<GetToDoDto>>
{
    private readonly LoggerExtension<GetTodoByIdQueryHandler> _logger = new(logger);
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<BaseResponse<GetToDoDto>> Handle(GetTodoByIdQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetToDoDto>();
        try
        {
            var todo = await _todoQueries.FindByIdAsync(request.Id, cancellationToken);

            if (todo is not null)
            {
                response.Data = todo;
                response.Success = true;
                response.Message = "Query succeed!";
            }
            else
            {
                response.Success = false;
                response.Message = "No result found!";
            }
        }
        catch (Exception ex)
        {
            response.Error = ex;

            _logger.LogException($"Get todo failed: {ex}", DateTime.Now);
        }

        return response;
    }
}
