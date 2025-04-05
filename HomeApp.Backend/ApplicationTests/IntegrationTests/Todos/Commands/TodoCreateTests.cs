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
        result.IsSuccess.Should().BeTrue("a valid todo should be created");
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
        result.IsSuccess.Should().BeTrue("todo with group should be created");
    }

    [Fact]
    public async Task CreateAsync_ShouldNotCreateWithoutPersonId()
    {
        // Arrange
        var todo = await CreateDummyTodos.GenereateDummyTodo();

        // Act
        var result = await TodoCommands.CreateAsync(todo, default);

        // Assert
        result.IsFailure.Should().BeTrue("no valid personId was provided");
        result.Error.Code.Should().Be("Todo.CreateFailedWithMessage");
        result.Error.Description.Should().Contain("person", "the error should describe the missing personId");
    }

    [Fact]
    public async Task CreateAsync_Fails_WhenTodoIsNull()
    {
        // Act
        var result = await TodoCommands.CreateAsync(null, default);

        // Assert
        result.IsFailure.Should().BeTrue("todo is null and should not be created");
        result.Error.Code.Should().Be("Todo.CreateFailedWithMessage");
        result.Error.Description.Should().Contain("null", "null values should be rejected");
    }
}
