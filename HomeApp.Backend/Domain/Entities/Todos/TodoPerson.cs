using Domain.Entities.People;
using SharedKernel.ValueObjects;

namespace Domain.Entities.Todos;

public class TodoPerson
{
    public TodoPersonId TodoPersonId { get; set; }
    public DateTime CreatedAt { get; set; }
    public PersonId PersonId { get; set; }
    public TodoId TodoId { get; set; }

    public virtual Person Person { get; set; }
    public virtual Todo Todo { get; set; }
}
