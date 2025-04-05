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

        var todo = await CreateDummyTodos.CreateOneDummyTodoWithPersonId(null, initialLastModified);

        var updatedTodo = new Todo { Id = todo.Id, Name = "Updated Todo", Done = true, Priority = TodoPriority.High };

        // Act
        var result = await TodoCommands.UpdateAsync(updatedTodo, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var dbTodo = await DbContext.Todos.FindAsync(todo.Id);
        dbTodo.Should().NotBeNull();
        dbTodo!.Name.Should().Be(updatedTodo.Name);
        dbTodo.Done.Should().Be(updatedTodo.Done);
        dbTodo.Priority.Should().Be(updatedTodo.Priority);
        dbTodo.LastModified.Should().BeAfter(initialLastModified);
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoIsNull()
    {
        // Act
        var result = await TodoCommands.UpdateAsync(null, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(TodoErrors.UpdateFailedWithMessage("").Code);
        result.Error.Description.Should().Contain("null");
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoPriorityIsInvalid()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Low, LastModified = DateTime.UtcNow
        };

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        var invalidTodo = new Todo
        {
            Id = todo.Id,
            Name = "Test Todo",
            Done = false,
            Priority = (TodoPriority)(-1), // Invalid priority
            LastModified = DateTime.UtcNow.AddDays(2)
        };

        // Act
        var result = await TodoCommands.UpdateAsync(invalidTodo, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(TodoErrors.UpdateFailedWithMessage("").Code);
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoDoesNotExist()
    {
        // Arrange
        var todo = new Todo
        {
            Id = 999,
            Name = "Non-existing Todo",
            Done = false,
            Priority = TodoPriority.Low,
            LastModified = DateTime.UtcNow.AddDays(1)
        };

        // Act
        var result = await TodoCommands.UpdateAsync(todo, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(TodoErrors.UpdateFailed(todo.Id).Code);
        result.Error.Description.Should().Contain("999");
    }
}
