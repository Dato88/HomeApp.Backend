using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.Todos.Dtos;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Todos.Queries;

public class GetUserTodosQueryHandler(
    ITodoQueries todoQueries,
    IPersonFacade personFacade,
    ILogger<GetUserTodosQueryHandler> logger)
    : IRequestHandler<GetUserTodosQuery, BaseResponse<IEnumerable<GetToDoDto>>>
{
    private readonly LoggerExtension<GetUserTodosQueryHandler> _logger = new(logger);
    private readonly IPersonFacade _personFacade = personFacade;
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<BaseResponse<IEnumerable<GetToDoDto>>> Handle(GetUserTodosQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetToDoDto>>();
        try
        {
            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

            var todos = await _todoQueries.GetAllAsync(person.Id, cancellationToken);

            if (todos.Any())
            {
                response.Data = todos.Select(s => (GetToDoDto)s);
                response.Success = true;
                response.Message = "Query succeed!";
            }
            else
            {
                response.Success = false;
                response.Message = "No results found!";
            }
        }
        catch (Exception ex)
        {
            response.Error = ex;

            _logger.LogException($"Get todos failed: {ex}", DateTime.Now);
        }

        return response;
    }
}
