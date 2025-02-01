using HomeApp.DataAccess.enums;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;

namespace HomeApp.DataAccess.Tests.Helper.CreateDummyData;

public class CreateDummyTodos : BaseTest
{
    private readonly CreateDummyPeople _createDummyPeople;

    public CreateDummyTodos(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _createDummyPeople = new CreateDummyPeople(unitTestingApiFactory);

    public async Task<Todo> CreateOneDummyTodo(DateTimeOffset? dateTimeOffset = null)
    {
        var person = await _createDummyPeople.CreateOneDummyPerson();

        Todo todo = new CreateToDoDto
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Normal, PersonId = person.Id
        };

        if (dateTimeOffset.HasValue) todo.LastModified = dateTimeOffset.Value;

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        return todo;
    }

    public async Task<Todo> CreateOneDummyTodoWithGroupAndReturnsTodo(DateTimeOffset? dateTimeOffset = null)
    {
        var todo = await CreateOneDummyTodo();

        TodoGroupTodo todoGroupTodo = new() { TodoId = todo.Id, TodoGroup = new TodoGroup { Name = "New Group" } };

        DbContext.TodoGroupTodos.Add(todoGroupTodo);
        await DbContext.SaveChangesAsync();

        return todo;
    }
}
