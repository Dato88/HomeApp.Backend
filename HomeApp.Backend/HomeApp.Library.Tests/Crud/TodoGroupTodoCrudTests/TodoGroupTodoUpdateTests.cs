namespace HomeApp.Library.Tests.Crud.TodoGroupTodoCrudTests;

public class TodoGroupTodoUpdateTests : BaseTodoGroupTodoCrudTest
{
    [Fact]
    public async Task UpdateAsync_UpdatesTodoGroupTodoInContext()
    {
        // Arrange
        var todoGroupTodo = new TodoGroupTodo { TodoGroupId = 1, TodoId = 1 };

        _context.TodoGroupTodos.Add(todoGroupTodo);
        await _context.SaveChangesAsync();

        var updatedTodoGroupTodo = new TodoGroupTodo { Id = todoGroupTodo.Id, TodoGroupId = 2, TodoId = 2 };

        // Act
        await _todoGroupTodoCrud.UpdateAsync(updatedTodoGroupTodo, default);

        // Assert
        var result = await _context.TodoGroupTodos.FindAsync(todoGroupTodo.Id);
        result.Should().NotBeNull();
        result.TodoGroupId.Should().Be(updatedTodoGroupTodo.TodoGroupId);
        result.TodoId.Should().Be(updatedTodoGroupTodo.TodoId);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoGroupTodoIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _todoGroupTodoCrud.UpdateAsync(null, default));

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoGroupTodoDoesNotExist()
    {
        // Arrange
        var nonExistingTodoGroupTodo = new TodoGroupTodo
        {
            Id = 999, // Nicht existierende ID
            TodoGroupId = 1,
            TodoId = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _todoGroupTodoCrud.UpdateAsync(nonExistingTodoGroupTodo, default));
    }
}
