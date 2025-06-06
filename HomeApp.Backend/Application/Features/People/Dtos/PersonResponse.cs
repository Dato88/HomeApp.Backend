using Domain.Entities.People;

namespace Application.Features.People.Dtos;

public sealed record PersonResponse(int id, string? username, string firstName, string lastName, string email)
{
    public PersonResponse() : this(0, null, string.Empty, string.Empty, string.Empty)
    {
    }

    public int Id { get; set; } = id;
    public string? Username { get; set; } = username;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;

    public static implicit operator PersonResponse?(Person? item) => item is not null
        ? new PersonResponse(item.Id, item.Username, item.FirstName, item.LastName, item.Email)
        : null;
}
