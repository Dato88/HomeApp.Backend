using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Todos;

public class TodoPerson : IAudited
{
    public int TodoPersonId { get; set; }

    public int PersonId { get; set; }
    public int TodoId { get; set; }

    public virtual Person? Person { get; set; }
    public virtual Todo? Todo { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
}
