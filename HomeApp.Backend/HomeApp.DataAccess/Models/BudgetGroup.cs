﻿#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("BudgetGroups")]
public class BudgetGroup : BaseClass
{
    [Required] public int PersonId { get; set; }

    [Required] public int Index { get; set; }

    [Required] [StringLength(150)] public string Name { get; set; }

    public virtual Person Person { get; set; }
    public virtual ICollection<BudgetCell> BudgetCells { get; set; } = new HashSet<BudgetCell>();
}
