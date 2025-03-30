using Domain.Entities.Todos;

namespace ApplicationTests.IntegrationTests.Todos.Commands;

public class TodoCreateTests : BaseTodoCommandsTest
{
    public TodoCreateTests(UnitTestingApiFactory unitTestingApiFactory) :
        base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateAsync_AddsTodoToContext()
    {
        // Arrange
        var person = await CreateDummyPeople.CreateDummyPersonAsync();
        var todo = await CreateDummyTodos.GenereateDummyTodo(person.Id);

        // Act
        await TodoCommands.CreateAsync(todo, default);

        // Assert
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
        var createdTodoId = await TodoCommands.CreateAsync(newTodo, default);

        var createdTodoExists = DbContext.Todos.Any(x => x.Id == createdTodoId);

        // Assert
        Assert.True(createdTodoExists);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotCreateWithoutPersonId()
    {
        // Arrange
        var todo = await CreateDummyTodos.GenereateDummyTodo();

        // Act
        CancellationToken cancellationToken = new();

        Func<Task> action = async () => await TodoCommands.CreateAsync(todo, cancellationToken);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Todo can`t be created without personId.");
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenTodoIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await TodoCommands.CreateAsync(null, default));
}
