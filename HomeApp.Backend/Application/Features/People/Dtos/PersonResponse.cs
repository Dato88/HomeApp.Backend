using Domain.Entities.People;
using SharedKernel.ValueObjects;

namespace Application.Features.People.Dtos;

public sealed record PersonResponse(
    PersonId PersonId,
    string? Username,
    string FirstName,
    string LastName,
    string Email)
{
    public static implicit operator PersonResponse?(Person? item) => item is not null
        ? new PersonResponse(item.PersonId, item.Username, item.FirstName, item.LastName, item.Email)
        : null;
}
