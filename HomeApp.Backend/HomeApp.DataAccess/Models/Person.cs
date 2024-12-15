#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("People")]
public class Person : BaseClass
{
    [StringLength(150)] public string? Username { get; set; }

    [Required] [StringLength(150)] public string FirstName { get; set; }

    [Required] [StringLength(150)] public string LastName { get; set; }

    [Required] [StringLength(150)] public string Email { get; set; }

    [Required] [StringLength(36)] public string UserId { get; set; }
}
