#nullable disable
namespace HomeApp.DataAccess.Models
{
    [Table("TodoUserMapping")]
    public partial class TodoUserMapping : BaseClass
    {
        [Required] public int UserId { get; set; }

        [Required] public int TodoId { get; set; }

        public virtual Person Person { get; set; }
        public virtual Todo Todo { get; set; }
    }
}