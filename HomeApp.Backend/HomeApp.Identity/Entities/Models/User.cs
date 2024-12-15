﻿using HomeApp.DataAccess.Models;

namespace HomeApp.Identity.Entities.Models;

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
            Email = item.Email,
            UserId = item.Id
        };
}
