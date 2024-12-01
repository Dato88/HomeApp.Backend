using System.ComponentModel.DataAnnotations;

namespace HomeApp.Identity.Entities.DataTransferObjects.ResetPassword;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string? Email { get; set; }

    public string? Token { get; set; }
}