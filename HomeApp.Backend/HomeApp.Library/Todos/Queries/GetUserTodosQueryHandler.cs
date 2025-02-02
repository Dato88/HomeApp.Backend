using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using HomeApp.Library.Models.BaseModels;
using MediatR;
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

            response.Data = await _todoQueries.GetAllAsync(person.Id, cancellationToken);

            if (response.Data is not null)
            {
                response.Succcess = true;
                response.Message = "Query succeed!";
            }
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;

            _logger.LogException($"Get todo failed: {ex}", DateTime.Now);
        }

        return response;
    }
}
