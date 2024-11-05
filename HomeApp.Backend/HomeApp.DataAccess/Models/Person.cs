#nullable disable
namespace HomeApp.DataAccess.Models
{
    [Table("Users")]
    public partial class Person : BaseClass
    {
        [Required] [StringLength(150)] public string Username { get; set; }

        [Required] [StringLength(150)] public string FirstName { get; set; }

        [Required] [StringLength(150)] public string LastName { get; set; }

        [Required] [StringLength(150)] public string Password { get; set; }

        [Required] [StringLength(150)] public string Email { get; set; }

        public DateTime? LastLogin { get; set; }
    }
}