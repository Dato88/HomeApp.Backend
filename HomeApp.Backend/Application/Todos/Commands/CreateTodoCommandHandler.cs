using Application.Abstractions.Logging;
using Application.Common.Interfaces.People;
using Application.Common.Interfaces.Todos;
using MediatR;
using SharedKernel;

namespace Application.Todos.Commands;

public class CreateTodoCommandHandler(
    ICommonPersonQueries commonPersonQueries,
    ITodoCommands todoCommands,
    IAppLogger<CreateTodoCommandHandler> logger) : IRequestHandler<CreateTodoCommand, BaseResponse<int>>
{
    private readonly ICommonPersonQueries _commonPersonQueries = commonPersonQueries;
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<BaseResponse<int>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();
        try
        {
            var person = await _commonPersonQueries.GetUserPersonAsync(cancellationToken);

            if (person == null)
            {
                response.Success = false;
                response.Message = "Failed to get user for create Todo!";

                logger.LogError("Create todos failed. User Person is Null.");

                return response;
            }

            request.PersonId = person.Id;

            response.Data = await _todoCommands.CreateAsync(request, cancellationToken);

            if (response.Data > 0)
            {
                response.Success = true;
                response.Message = "Create succeed!";
            }

            logger.LogInformation($"Creating todo: {response.Data}");
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;

            logger.LogError($"Creating todo failed: {ex.Message}");
        }


        return response;
    }
}
