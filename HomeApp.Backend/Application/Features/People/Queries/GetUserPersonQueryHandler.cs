using Application.Abstractions.Logging;
using Application.Features.People.Dtos;
using Domain.Entities.People;
using MediatR;
using SharedKernel;

namespace Application.Features.People.Queries;

public class GetUserPersonQueryHandler(
    IPersonQueries personQueries,
    IAppLogger<GetUserPersonQueryHandler> logger) : IRequestHandler<GetUserPersonQuery, Result<PersonResponse>>
{
    private readonly IPersonQueries _personQueries = personQueries;

    public async Task<Result<PersonResponse>> Handle(GetUserPersonQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var person = await _personQueries.GetUserPersonAsync(cancellationToken);

            if (person == null) return Result.Failure<PersonResponse>(PersonErrors.NotFound);

            return person;
        }
        catch (Exception ex)
        {
            logger.LogError($"Get person failed: {ex}");

            return Result.Failure<PersonResponse>(PersonErrors.NotFound);
        }
    }
}
