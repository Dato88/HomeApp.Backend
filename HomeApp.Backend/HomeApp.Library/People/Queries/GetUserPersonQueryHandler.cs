﻿using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.People.Dtos;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.People.Queries;

public class GetUserPersonQueryHandler(
    IPersonFacade personFacade,
    ILogger<GetUserPersonQueryHandler> logger) : IRequestHandler<GetUserPersonQuery, BaseResponse<PersonDto>>
{
    private readonly LoggerExtension<GetUserPersonQueryHandler> _logger = new(logger);
    private readonly IPersonFacade _personFacade = personFacade;

    public async Task<BaseResponse<PersonDto>> Handle(GetUserPersonQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<PersonDto>();
        try
        {
            var person = await _personFacade.GetUserPersonAsync(cancellationToken);

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
