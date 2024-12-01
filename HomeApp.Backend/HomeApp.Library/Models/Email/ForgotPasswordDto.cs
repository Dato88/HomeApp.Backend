using System.ComponentModel.DataAnnotations;

namespace HomeApp.Library.Models.Email;

public class ForgotPasswordDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "ClientURI is required")]
    public string? ClientURI { get; set; }
}