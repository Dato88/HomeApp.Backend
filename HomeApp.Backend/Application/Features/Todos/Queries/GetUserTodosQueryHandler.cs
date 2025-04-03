using Application.Abstractions.Logging;
using Application.Features.People.Queries;
using Application.Features.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public class GetUserTodosQueryHandler(
    ITodoQueries todoQueries,
    IPersonQueries personQueries,
    IAppLogger<GetUserTodosQueryHandler> logger)
    : IRequestHandler<GetUserTodosQuery, BaseResponse<IEnumerable<GetToDoResponse>>>
{
    private readonly IPersonQueries _personQueries = personQueries;
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<BaseResponse<IEnumerable<GetToDoResponse>>> Handle(GetUserTodosQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<GetToDoResponse>>();
        try
        {
            var person = await _personQueries.GetUserPersonAsync(cancellationToken);

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
                response.Data = todos.Select(s => (GetToDoResponse)s);
                response.Message = "Query succeed!";
            }
            else
            {
                response.Message = "No results found!";
            }

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.Error = ex;

            logger.LogError($"Get todos failed: {ex}");
        }

        return response;
    }
}
