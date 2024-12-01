using System.ComponentModel.DataAnnotations;
using HomeApp.Identity.Entities.Models;

namespace HomeApp.Identity.Entities.DataTransferObjects.Register;

public class RegisterUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    public string? ClientUri { get; set; }

    public static implicit operator User(RegisterUserDto item)
    {
        return new User
        {
            FirstName = item.FirstName,
            LastName = item.LastName,
            Email = item.Email,
            UserName = item.Email
        };
    }

    public static implicit operator RegisterUserDto(User item)
    {
        return new RegisterUserDto
        {
            FirstName = item.FirstName,
            LastName = item.LastName,
            Email = item.UserName
        };
    }
}