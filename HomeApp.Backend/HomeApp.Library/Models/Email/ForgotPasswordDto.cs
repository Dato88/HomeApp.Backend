using System.ComponentModel.DataAnnotations;

namespace HomeApp.Library.Models.Email;

public class ForgotPasswordDto
{
    [Required] [EmailAddress] public string? Email { get; set; }
    [Required] public string? ClientURI { get; set; }
}