using Domain.Entities.Todos;

namespace ApplicationTests.IntegrationTests.Todos.Commands;

public class TodoCreateTests : BaseTodoCommandsTest
{
    public TodoCreateTests(UnitTestingApiFactory unitTestingApiFactory)
        : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateAsync_AddsTodoToContext()
    {
        // Arrange
        var person = await CreateDummyPeople.CreateDummyPersonAsync();
        var todo = await CreateDummyTodos.GenereateDummyTodo(person.Id);

        // Act
        var result = await TodoCommands.CreateAsync(todo, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Contains(todo, DbContext.Todos);
    }

    [Fact]
    public async Task CreateAsync_Should_Also_Create_TodoGroupTodo()
    {
        // Arrange
        var person = await CreateDummyPeople.CreateDummyPersonAsync();

        var todoGroup = new TodoGroup { Name = "TestGroup" };
        DbContext.TodoGroups.Add(todoGroup);
        await DbContext.SaveChangesAsync();

        var newTodo = await CreateDummyTodos.GenereateDummyTodo(person.Id);
        newTodo.TodoGroupTodo = new TodoGroupTodo { TodoGroupId = todoGroup.Id };

        // Act
        var result = await TodoCommands.CreateAsync(newTodo, default);

        // Assert
        Assert.True(result.IsSuccess);
        var createdTodoExists = DbContext.Todos.Any(x => x.Id == result.Value);
        Assert.True(createdTodoExists);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotCreateWithoutPersonId()
    {
        // Arrange
        var todo = await CreateDummyTodos.GenereateDummyTodo();

        // Act
        var result = await TodoCommands.CreateAsync(todo, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Todo.CreateFailedWithMessage", result.Error.Code);
        Assert.Contains("person", result.Error.Description, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreateAsync_Fails_WhenTodoIsNull()
    {
        // Act
        var result = await TodoCommands.CreateAsync(null, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Todo.CreateFailedWithMessage", result.Error.Code);
        Assert.Contains("null", result.Error.Description, StringComparison.OrdinalIgnoreCase);
    }
}
