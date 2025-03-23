using Domain.Entities.People;
using SharedKernel;

namespace Domain.Entities.Todos;

public class TodoPerson : BaseClass
{
    public int PersonId { get; set; }
    public int TodoId { get; set; }

    public virtual Person Person { get; set; }
    public virtual Todo Todo { get; set; }
}
