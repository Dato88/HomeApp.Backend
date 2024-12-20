using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Tests.Crud.TodoCrudTests;

public class TodoUpdateTests : BaseTodoTest
{
    [Fact]
    public async Task UpdateAsync_UpdatesTodoInContext()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Initial Todo",
            Done = false,
            Priority = TodoPriority.Low,
            ExecutionDate = DateTime.Now.AddDays(1)
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var updatedTodo = new Todo
        {
            Id = todo.Id,
            Name = "Updated Todo",
            Done = true,
            Priority = TodoPriority.High,
            ExecutionDate = DateTime.Now.AddDays(2)
        };

        // Act
        await _todoCrud.UpdateAsync(updatedTodo, default);

        // Assert
        var result = await _context.Todos.FindAsync(todo.Id);
        result.Should().NotBeNull();
        result.Name.Should().Be(updatedTodo.Name);
        result.Done.Should().Be(updatedTodo.Done);
        result.Priority.Should().Be(updatedTodo.Priority);
        result.ExecutionDate.Should().Be(updatedTodo.ExecutionDate);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _todoCrud.UpdateAsync(null, default));

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoPriorityIsInvalid()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Low, ExecutionDate = DateTime.Now.AddDays(1)
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        var invalidTodo = new Todo
        {
            Id = todo.Id,
            Name = "Test Todo",
            Done = false,
            Priority = (TodoPriority)(-1), // Invalid priority
            ExecutionDate = DateTime.Now.AddDays(2)
        };

        // Act & Assert
        var action = async () => await _todoCrud.UpdateAsync(invalidTodo, default);
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"Priority ('{invalidTodo.Priority}') must be a non-negative and non-zero value. (Parameter 'Priority')Actual value was {invalidTodo.Priority}.");
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
            ExecutionDate = DateTime.Now.AddDays(1)
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _todoCrud.UpdateAsync(todo, default));
    }
}
