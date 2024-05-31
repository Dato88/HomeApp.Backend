#nullable disable
namespace HomeApp.DataAccess.Models
{
    [Table("BudgetCells")]
    public partial class BudgetCell : BaseClass
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int BudgetRowId { get; set; }

        [Required]
        public int BudgetColumnId { get; set; }

        [Required]
        public int BudgetGroupId { get; set; }

        public virtual User User { get; set; }
        public virtual BudgetColumn BudgetColumn { get; set; }
        public virtual BudgetGroup BudgetGroup { get; set; }
        public virtual BudgetRow BudgetRow { get; set; }
    }
}