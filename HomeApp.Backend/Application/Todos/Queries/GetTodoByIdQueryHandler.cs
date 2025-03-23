using Application.Common.Interfaces.Todos;
using Application.Todos.Dtos;
using HomeApp.Library.Logger;
using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel;

namespace Application.Todos.Queries;

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
