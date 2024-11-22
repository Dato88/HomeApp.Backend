using System.ComponentModel.DataAnnotations;

namespace HomeApp.Identity.Models.Authentication;

public class UserForAuthenticationDto
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}