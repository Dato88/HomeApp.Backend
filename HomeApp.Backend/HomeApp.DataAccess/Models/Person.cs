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

    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
    public virtual ICollection<BudgetGroup> BudgetGroups { get; set; } = new HashSet<BudgetGroup>();
    public virtual ICollection<BudgetRow> BudgetRows { get; set; } = new HashSet<BudgetRow>();
    public virtual ICollection<TodoUserMapping> TodosUser { get; set; } = new HashSet<TodoUserMapping>();
}
