using Application.Abstractions.Logging;
using Application.Common.Interfaces.People;
using Application.People.Dtos;
using MediatR;
using SharedKernel;

namespace Application.People.Queries;

public class GetUserPersonQueryHandler(
    ICommonPersonQueries commonPersonQueries,
    IAppLogger<GetUserPersonQueryHandler> logger) : IRequestHandler<GetUserPersonQuery, BaseResponse<PersonDto>>
{
    private readonly ICommonPersonQueries _commonPersonQueries = commonPersonQueries;

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

            logger.LogException($"Get person failed: {ex}");
        }

        return response;
    }
}
