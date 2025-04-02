using Application.Abstractions.Logging;
using Application.Features.People.Queries;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Commands;

public class CreateTodoCommandHandler(
    IPersonQueries personQueries,
    ITodoCommands todoCommands,
    IAppLogger<CreateTodoCommandHandler> logger) : IRequestHandler<CreateTodoCommand, BaseResponse<int>>
{
    private readonly IPersonQueries _personQueries = personQueries;
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<BaseResponse<int>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();
        try
        {
            var person = await _personQueries.GetUserPersonAsync(cancellationToken);

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
