using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;

namespace ApplicationTests.IntegrationTests.Todos.Commands;

public class TodoUpdateTests : BaseTodoCommandsTest
{
    public TodoUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task UpdateAsync_UpdatesTodoInContext()
    {
        // Arrange
        var initialLastModified = DateTime.UtcNow.AddDays(-7);

        var todo = await TodosDataSeeder.CreateOneDummyTodoWithPersonId(null, initialLastModified);

        var updatedTodo = new Todo
        {
            TodoId = todo.TodoId, Title = "Updated Todo", Done = true, Priority = TodoPriority.High
        };

        // Act
        var result = await TodoCommands.UpdateAsync(updatedTodo, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var dbTodo = await DbContext.Todos.FindAsync(todo.TodoId);
        dbTodo.Should().NotBeNull();
        dbTodo!.Title.Should().Be(updatedTodo.Title);
        dbTodo.Done.Should().Be(updatedTodo.Done);
        dbTodo.Priority.Should().Be(updatedTodo.Priority);
        dbTodo.UpdatedAt.Should().BeAfter(initialLastModified);
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoIsNull()
    {
        // Act
        var result = await TodoCommands.UpdateAsync(null, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(TodoErrors.UpdateFailedWithMessage("Todo is null"));
        result.Error.Description.Should().Contain("null");
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoPriorityIsInvalid()
    {
        // Arrange
        var todo = new Todo
        {
            Title = "Test Todo", Done = false, Priority = TodoPriority.Low, UpdatedAt = DateTime.UtcNow
        };

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        var invalidTodo = new Todo
        {
            TodoId = todo.TodoId,
            Title = "Test Todo",
            Done = false,
            Priority = (TodoPriority)(-1), // Invalid priority
            UpdatedAt = DateTime.UtcNow.AddDays(2)
        };

        // Act
        var result = await TodoCommands.UpdateAsync(invalidTodo, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(TodoErrors.UpdateFailedWithMessage("Priority is invalid"));
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoDoesNotExist()
    {
        // Arrange
        var todo = new Todo
        {
            TodoId = 999,
            Title = "Non-existing Todo",
            Done = false,
            Priority = TodoPriority.Low,
            UpdatedAt = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await TodoCommands.UpdateAsync(todo, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(TodoErrors.UpdateFailed(todo.TodoId));
        result.Error.Description.Should().Contain("999");
    }
}
