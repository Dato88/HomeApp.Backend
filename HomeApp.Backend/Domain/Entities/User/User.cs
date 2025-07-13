using Domain.Entities.People;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.User;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public static implicit operator Person(User item) =>
        new()
        {
            Username = item.Email,
            FirstName = item.FirstName,
            LastName = item.LastName,
            Email = new UserEmail(item.Email),
            UserId = item.Id
        };
}
