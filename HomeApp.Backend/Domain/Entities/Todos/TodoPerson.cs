using Domain.Entities.People;

namespace Domain.Entities.Todos;

public class TodoPerson
{
    public int TodoPersonId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int PersonId { get; set; }
    public int TodoId { get; set; }

    public virtual Person Person { get; set; }
    public virtual Todo Todo { get; set; }
}
