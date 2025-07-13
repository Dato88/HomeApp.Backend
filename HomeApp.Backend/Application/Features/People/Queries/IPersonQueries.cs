using Application.Features.People.Dtos;
using Domain.ValueObjects;

namespace Application.Features.People.Queries;

public interface IPersonQueries
{
    Task<PersonResponse?> GetUserPersonAsync(CancellationToken cancellationToken);
    Task<PersonResponse?> GetPersonByEmailAsync(string email, CancellationToken cancellationToken);
}
