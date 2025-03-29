using Application.Abstractions.Logging;
using Application.Common.Interfaces.People;
using Application.Common.Interfaces.Todos;
using Application.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Todos.Queries;

public class GetUserTodosQueryHandler(
    ITodoQueries todoQueries,
    ICommonPersonQueries commonPersonQueries,
    IAppLogger<GetUserTodosQueryHandler> logger)
    : IRequestHandler<GetUserTodosQuery, BaseResponse<IEnumerable<GetToDoDto>>>
{
    private readonly ICommonPersonQueries _commonPersonQueries = commonPersonQueries;
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<BaseResponse<IEnumerable<GetToDoDto>>> Handle(GetUserTodosQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetToDoDto>>();
        try
        {
            var person = await _commonPersonQueries.GetUserPersonAsync(cancellationToken);

            if (person == null)
            {
                response.Success = false;
                response.Message = "Failed to get user!";

                logger.LogError("Get todos failed. User Person is Null.");

                return response;
            }

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

            logger.LogCritical($"Get todos failed: {ex}");
        }

        return response;
    }
}
