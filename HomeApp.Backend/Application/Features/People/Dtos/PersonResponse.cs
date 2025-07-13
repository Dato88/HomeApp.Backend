using Domain.Entities.People;

namespace Application.Features.People.Dtos;

public sealed record PersonResponse(
    int PersonId,
    string? Username,
    string FirstName,
    string LastName,
    string Email)
{
    public static implicit operator PersonResponse?(Person? item) => item is not null
        ? new PersonResponse(item.PersonId, item.Username, item.FirstName, item.LastName, item.Email)
        : null;
}
