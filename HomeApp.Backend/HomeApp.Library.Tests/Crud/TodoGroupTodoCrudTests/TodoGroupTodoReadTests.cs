namespace HomeApp.Library.Tests.Crud.TodoGroupTodoCrudTests;

public class TodoGroupTodoReadTests : BaseTodoGroupTodoCrudTest
{
    [Fact]
    public async Task FindByIdAsync_ReturnsTodoGroupTodo_WhenExists()
    {
        // Arrange
        TodoGroupTodo todoGroupTodo = new() { TodoGroupId = 1, TodoId = 1 };

        _context.TodoGroupTodos.Add(todoGroupTodo);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoGroupTodoCrud.FindByIdAsync(todoGroupTodo.Id, default);

        // Assert
        result.Should().BeEquivalentTo(todoGroupTodo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsInvalid(int invalidId)
    {
        // Act & Assert
        Func<Task> action = async () => await _todoGroupTodoCrud.FindByIdAsync(invalidId, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"{nameof(TodoGroupTodo.Id)} ('{invalidId}') must be a non-negative and non-zero value. (Parameter '{nameof(TodoGroupTodo.Id)}')\nActual value was {invalidId}.");
    }

    [Fact]
    public async Task FindByIdAsync_ThrowsException_WhenTodoGroupTodoDoesNotExist()
    {
        // Arrange
        var nonExistingId = 999;

        // Act
        Func<Task> action = async () => await _todoGroupTodoCrud.FindByIdAsync(nonExistingId, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoGroupTodoMessage.TodoGroupTodoNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTodoGroupTodos()
    {
        // Arrange
        List<TodoGroupTodo> todoGroupTodos = new()
        {
            new TodoGroupTodo { TodoGroupId = 1, TodoId = 1 }, new TodoGroupTodo { TodoGroupId = 2, TodoId = 2 }
        };

        _context.TodoGroupTodos.AddRange(todoGroupTodos);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoGroupTodoCrud.GetAllAsync(default);

        // Assert
        result.Should().BeEquivalentTo(todoGroupTodos);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoTodoGroupTodosExist()
    {
        // Act
        var result = await _todoGroupTodoCrud.GetAllAsync(default);

        // Assert
        result.Should().BeEmpty();
    }
}
