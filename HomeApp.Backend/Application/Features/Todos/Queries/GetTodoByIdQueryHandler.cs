using Application.Abstractions.Logging;
using Application.Features.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public class GetTodoByIdQueryHandler(
    ITodoQueries todoQueries,
    IAppLogger<GetTodoByIdQueryHandler> logger)
    : IRequestHandler<GetTodoByIdQuery, BaseResponse<GetToDoResponse>>
{
    private readonly ITodoQueries _todoQueries = todoQueries;

    public async Task<BaseResponse<GetToDoResponse>> Handle(GetTodoByIdQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<GetToDoResponse>();
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

            logger.LogError($"Get todo failed: {ex}");
        }

        return response;
    }
}
