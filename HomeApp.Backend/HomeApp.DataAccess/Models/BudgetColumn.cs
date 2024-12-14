#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("BudgetColumns")]
public partial class BudgetColumn : BaseClass
{
    public BudgetColumn() => BudgetCells = new HashSet<BudgetCell>();

    [Required]
    public int Index { get; set; }

    [Required]
    [StringLength(150)]
    public string Name { get; set; }

    public virtual ICollection<BudgetCell> BudgetCells { get; set; }
}
