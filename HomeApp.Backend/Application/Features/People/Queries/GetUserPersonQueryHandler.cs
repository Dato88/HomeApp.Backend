using Application.Abstractions.Logging;
using Application.Common.Interfaces.People;
using Application.Features.People.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.People.Queries;

public class GetUserPersonQueryHandler(
    IPersonQueries personQueries,
    IAppLogger<GetUserPersonQueryHandler> logger) : IRequestHandler<GetUserPersonQuery, BaseResponse<PersonDto>>
{
    private readonly IPersonQueries _personQueries = personQueries;

    public async Task<BaseResponse<PersonDto>> Handle(GetUserPersonQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<PersonDto>();
        try
        {
            var person = await _personQueries.GetUserPersonAsync(cancellationToken);

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

            logger.LogError($"Get person failed: {ex}");
        }

        return response;
    }
}
