namespace HomeApp.Library.People.Dtos;

public class PersonDto(int id, string? username, string firstName, string lastName, string email)
{
    public PersonDto() : this(0, null, string.Empty, string.Empty, string.Empty)
    {
    }

    public int Id { get; set; } = id;
    public string? Username { get; set; } = username;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;

    public static implicit operator PersonDto(Person item) =>
        new(item.Id, item.Username, item.FirstName, item.LastName, item.Email);
}
