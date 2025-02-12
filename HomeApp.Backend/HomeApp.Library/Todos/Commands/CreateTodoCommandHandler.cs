using HomeApp.DataAccess.Cruds.Interfaces.Todos;
using HomeApp.Library.Facades.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Todos.Commands;

public class CreateTodoCommandHandler(
    IPersonFacade personFacade,
    ITodoCommands todoCommands,
    ILogger<CreateTodoCommandHandler> logger) : IRequestHandler<CreateTodoCommand, BaseResponse<int>>
{
    private readonly LoggerExtension<CreateTodoCommandHandler> _logger = new(logger);
    private readonly IPersonFacade _personFacade = personFacade;
    private readonly ITodoCommands _todoCommands = todoCommands;

    public async Task<BaseResponse<int>> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();
        try
        {
            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

            if (person == null)
            {
                response.Success = false;
                response.Message = "Failed to get user for create Todo!";

                _logger.LogException("Create todos failed. User Person is Null.", DateTime.Now);

                return response;
            }

            request.PersonId = person.Id;

            response.Data = await _todoCommands.CreateAsync(request, cancellationToken);

            if (response.Data > 0)
            {
                response.Success = true;
                response.Message = "Create succeed!";
            }

            _logger.LogInformation($"Creating todo: {response.Data}", DateTime.Now);
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;

            _logger.LogException($"Creating todo failed: {ex.Message}", DateTime.Now);
        }


        return response;
    }
}
