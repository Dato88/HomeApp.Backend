using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Logger;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Todos.Commands;

public class GetUserTodosQueryHandler(
    ITodoQueries todoQueries,
    IPersonFacade personFacade,
    ILogger<GetUserTodosQueryHandler> logger)
    : IRequestHandler<GetUserTodosQuery, IEnumerable<GetToDoDto>>
{
    private readonly LoggerExtension<GetUserTodosQueryHandler> _logger = new(logger);
    private readonly IPersonFacade _personFacade = personFacade;
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<IEnumerable<GetToDoDto>> Handle(GetUserTodosQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

            return await _todoQueries.GetAllAsync(person.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogException($"Get todo failed: {ex}", DateTime.Now);

            return Enumerable.Empty<GetToDoDto>();
        }
    }
}
