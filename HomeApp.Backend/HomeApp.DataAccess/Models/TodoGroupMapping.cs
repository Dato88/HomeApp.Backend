#nullable disable
namespace HomeApp.DataAccess.Models
{
    [Table("TodoGroupMapping")]
    public partial class TodoGroupMapping : BaseClass
    {
        [Required]
        public int TodoId { get; set; }

        [Required]
        public int TodoGroupId { get; set; }

        public virtual Todo Todo { get; set; }
        public virtual TodoGroup TodoGroup { get; set; }
    }
}