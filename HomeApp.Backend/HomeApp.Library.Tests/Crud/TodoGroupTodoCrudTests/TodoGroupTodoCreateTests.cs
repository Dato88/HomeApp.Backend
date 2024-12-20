﻿namespace HomeApp.Library.Tests.Crud.TodoGroupTodoCrudTests;

public class TodoGroupTodoCreateTests : BaseTodoGroupTodoCrudTest
{
    [Fact]
    public async Task CreateAsync_AddsTodoGroupTodoToContext()
    {
        // Arrange
        TodoGroupTodo todoGroupTodo = new() { TodoGroupId = 1, TodoId = 1 };

        // Act
        CancellationToken cancellationToken = new();
        await _todoGroupTodoCrud.CreateAsync(todoGroupTodo, cancellationToken);

        // Assert
        Assert.Contains(todoGroupTodo, _context.TodoGroupTodos);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenTodoGroupTodoIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _todoGroupTodoCrud.CreateAsync(null, default));
}
