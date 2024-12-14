#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("BudgetRows")]
public partial class BudgetRow : BaseClass
{
    [Required] public int UserId { get; set; }

    [Required] public int Index { get; set; }

    [Required] public int Year { get; set; }

    [Required][StringLength(150)] public string Name { get; set; }

    public virtual Person Person { get; set; }
    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
}
