namespace HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;

public class CreateToDoPersonDto
{
    public CreateToDoPersonDto(int personId, int todoId)
    {
        PersonId = personId;
        TodoId = todoId;
    }

    public int PersonId { get; set; }

    public int TodoId { get; set; }

    public static implicit operator TodoPerson(CreateToDoPersonDto item) =>
        new() { PersonId = item.PersonId, TodoId = item.TodoId };
}
