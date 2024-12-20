#nullable disable
namespace HomeApp.DataAccess.Models;

[Table("TodoPeople")]
public class TodoPerson : BaseClass
{
    [Required] public int PersonId { get; set; }

    [Required] public int TodoId { get; set; }

    public virtual Person Person { get; set; }
    public virtual Todo Todo { get; set; }
}
