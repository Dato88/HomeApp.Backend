using HomeApp.Library.Common.Interfaces;
using HomeApp.Library.People.Dtos;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.People.Queries;

public class GetUserPersonQueryHandler(
    ICommonPersonQueries commonPersonQueries,
    ILogger<GetUserPersonQueryHandler> logger) : IRequestHandler<GetUserPersonQuery, BaseResponse<PersonDto>>
{
    private readonly ICommonPersonQueries _commonPersonQueries = commonPersonQueries;
    private readonly LoggerExtension<GetUserPersonQueryHandler> _logger = new(logger);

    public async Task<BaseResponse<PersonDto>> Handle(GetUserPersonQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<PersonDto>();
        try
        {
            var person = await _commonPersonQueries.GetUserPersonAsync(cancellationToken);

            if (person is not null)
            {
                response.Data = person;
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

            _logger.LogException($"Get person failed: {ex}", DateTime.Now);
        }

        return response;
    }
}
