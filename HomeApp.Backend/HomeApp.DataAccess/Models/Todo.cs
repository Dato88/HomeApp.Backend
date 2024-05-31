#nullable disable
namespace HomeApp.DataAccess.Models
{
    [Table("Todos")]
    public partial class Todo : BaseClass
    {
        public Todo()
        {
            TodoGroupMappings = new HashSet<TodoGroupMapping>();
            TodoUserMappings = new HashSet<TodoUserMapping>();
        }

        [Required]
        [StringLength(150)]
        public string TodoName { get; set; }

        [Required]
        public bool TodoDone { get; set; }

        [Required]
        public int TodoPriority { get; set; }

        public DateTime TodoExecutionDate { get; set; }

        public virtual ICollection<TodoGroupMapping> TodoGroupMappings { get; set; }

        public virtual ICollection<TodoUserMapping> TodoUserMappings { get; set; }
    }
}