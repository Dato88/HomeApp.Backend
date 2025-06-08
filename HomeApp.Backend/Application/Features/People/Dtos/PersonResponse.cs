using Domain.Entities.People;

namespace Application.Features.People.Dtos;

public sealed record PersonResponse(int Id, string? Username, string FirstName, string LastName, string Email)
{
    public static implicit operator PersonResponse?(Person? item) => item is not null
        ? new PersonResponse(item.Id, item.Username, item.FirstName, item.LastName, item.Email)
        : null;
}
