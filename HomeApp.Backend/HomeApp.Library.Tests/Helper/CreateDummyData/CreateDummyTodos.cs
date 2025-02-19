using Bogus;
using HomeApp.DataAccess.Enums;

namespace HomeApp.Library.Tests.Helper.CreateDummyData;

public class CreateDummyTodos : BaseTest
{
    private readonly CreateDummyPeople _createDummyPeople;
    private readonly Faker<Todo> _todoFaker;

    public CreateDummyTodos(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _createDummyPeople = new CreateDummyPeople(unitTestingApiFactory);

        Randomizer.Seed = new Random(1337);

        _todoFaker = new Faker<Todo>()
            .RuleFor(u => u.Name, f => f.Name.FirstName())
            .RuleFor(u => u.Done, f => f.Random.Bool())
            .RuleFor(u => u.Priority, f => f.PickRandom<TodoPriority>());
    }

    public async Task<Todo> GenereateDummyTodo(int? personId = null)
    {
        await Task.Delay(0);

        var todo = _todoFaker.Generate();

        if (personId.HasValue) todo.TodoPeople.Add(new TodoPerson { PersonId = personId.Value });

        return todo;
    }

    public async Task<Todo> CreateOneDummyTodoWithPersonId(int? personId = null, DateTime? dateTime = null)
    {
        if (personId is null)
            personId = (await _createDummyPeople.CreateDummyPersonAsync()).Id;

        var todo = await GenereateDummyTodo(personId);

        if (dateTime.HasValue) todo.LastModified = dateTime.Value;

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        var result = await DbContext.Todos.FindAsync(todo.Id);

        return result ?? new Todo();
    }

    public async Task<Todo> CreateOneDummyTodoWithGroupAndReturnsTodo(DateTimeOffset? dateTimeOffset = null)
    {
        var todo = await CreateOneDummyTodoWithPersonId();

        TodoGroupTodo todoGroupTodo = new() { TodoId = todo.Id, TodoGroup = new TodoGroup { Name = "New Group" } };

        DbContext.TodoGroupTodos.Add(todoGroupTodo);
        await DbContext.SaveChangesAsync();

        return todo;
    }
}
