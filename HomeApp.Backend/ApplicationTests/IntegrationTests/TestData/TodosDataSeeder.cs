using Bogus;
using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;
using SharedKernel.ValueObjects;

namespace ApplicationTests.IntegrationTests.TestData;

public class TodosDataSeeder : BaseTest
{
    private readonly PeopleDataSeeder _peopleDataSeeder;
    private readonly Faker<Todo> _todoFaker;

    public TodosDataSeeder(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _peopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);

        _todoFaker = new Faker<Todo>()
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Done, f => f.Random.Bool())
            .RuleFor(u => u.Priority, f => f.PickRandom<TodoPriority>());
    }

    public async Task<Todo> GenereateDummyTodo(PersonId? personId = null)
    {
        await Task.Delay(0);

        var todo = _todoFaker.Generate();

        if (personId.HasValue) todo.TodoPeople.Add(new TodoPerson { PersonId = personId.Value });

        return todo;
    }

    public async Task<Todo> CreateOneDummyTodoWithPersonId(PersonId? personId = null, DateTime? dateTime = null)
    {
        // if (userId is null)
        //     userId = (await _peopleDataSeeder.SeedPersonAsync()).UserId;

        var todo = await GenereateDummyTodo(personId);

        if (dateTime.HasValue) todo.LastModified = dateTime.Value;

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo> CreateOneDummyTodoWithGroupAndReturnsTodo(DateTimeOffset? dateTimeOffset = null)
    {
        var todo = await CreateOneDummyTodoWithPersonId();

        TodoGroupTodo todoGroupTodo = new() { TodoId = todo.TodoId, TodoGroup = new TodoGroup { Name = "New Group" } };

        DbContext.TodoGroupTodos.Add(todoGroupTodo);
        await DbContext.SaveChangesAsync();

        return todo;
    }
}
