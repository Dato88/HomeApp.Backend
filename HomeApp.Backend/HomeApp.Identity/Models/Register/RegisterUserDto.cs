using System.ComponentModel.DataAnnotations;

namespace HomeApp.Identity.Models.Register;

public class RegisterUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    public static implicit operator User(RegisterUserDto item)
    {
        return new()
        {
            FirstName = item.FirstName,
            LastName = item.LastName,
            Email = item.Email,
            UserName = item.Email
        };
    }
}