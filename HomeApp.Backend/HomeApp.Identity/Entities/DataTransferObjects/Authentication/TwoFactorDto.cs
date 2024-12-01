using System.ComponentModel.DataAnnotations;

namespace HomeApp.Identity.Entities.DataTransferObjects.Authentication;

public class TwoFactorDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Provider is required")]
    public string? Provider { get; set; }

    [Required(ErrorMessage = "Token is required")]
    public string? Token { get; set; }
}