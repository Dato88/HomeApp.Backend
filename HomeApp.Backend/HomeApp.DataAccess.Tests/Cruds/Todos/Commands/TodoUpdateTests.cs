using FluentAssertions;
using HomeApp.DataAccess.enums;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Tests.Helper;
using Xunit;

namespace HomeApp.DataAccess.Tests.Cruds.Todos.Commands;

public class TodoUpdateTests : BaseTodoTest
{
    public TodoUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task UpdateAsync_UpdatesTodoInContext()
    {
        // Arrange
        var initialLastModified = DateTimeOffset.UtcNow.AddDays(-1);

        var todo = new Todo
        {
            Name = "Initial Todo", Done = false, Priority = TodoPriority.Low, LastModified = initialLastModified
        };

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        var updatedTodo = new Todo { Id = todo.Id, Name = "Updated Todo", Done = true, Priority = TodoPriority.High };

        // Act
        await _todoCommands.UpdateAsync(updatedTodo, default);

        // Assert
        var result = await DbContext.Todos.FindAsync(todo.Id);
        result.Should().NotBeNull();
        result.Name.Should().Be(updatedTodo.Name);
        result.Done.Should().Be(updatedTodo.Done);
        result.Priority.Should().Be(updatedTodo.Priority);
        result.LastModified.Should().BeAfter(initialLastModified);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _todoCommands.UpdateAsync(null, default));

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoPriorityIsInvalid()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Low, LastModified = DateTimeOffset.UtcNow
        };

        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        var invalidTodo = new Todo
        {
            Id = todo.Id,
            Name = "Test Todo",
            Done = false,
            Priority = (TodoPriority)(-1), // Invalid priority
            LastModified = DateTimeOffset.UtcNow.AddDays(2)
        };

        // Act & Assert
        var action = async () => await _todoCommands.UpdateAsync(invalidTodo, default);
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"Priority ('{invalidTodo.Priority}') must be a non-negative value. (Parameter 'Priority')Actual value was {invalidTodo.Priority}.");
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoDoesNotExist()
    {
        var todo = new Todo
        {
            Id = 999, // Non-existing ID
            Name = "Non-existing Todo",
            Done = false,
            Priority = TodoPriority.Low,
            LastModified = DateTimeOffset.UtcNow.AddDays(1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _todoCommands.UpdateAsync(todo, default));
    }
}
