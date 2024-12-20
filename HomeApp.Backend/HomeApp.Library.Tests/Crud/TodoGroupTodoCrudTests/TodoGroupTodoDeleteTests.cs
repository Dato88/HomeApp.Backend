namespace HomeApp.Library.Tests.Crud.TodoGroupTodoCrudTests;

public class TodoGroupTodoDeleteTests : BaseTodoGroupTodoCrudTest
{
    [Fact]
    public async Task DeleteAsync_ShouldDeleteTodoGroupTodo_WhenTodoGroupTodoExists()
    {
        // Arrange
        TodoGroupTodo todoGroupTodo = new() { TodoGroupId = 1, TodoId = 1 };

        _context.TodoGroupTodos.Add(todoGroupTodo);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoGroupTodoCrud.DeleteAsync(todoGroupTodo.Id, default);

        // Assert
        result.Should().BeTrue();
        var deletedTodoGroupTodo = await _context.TodoGroupTodos.FindAsync(todoGroupTodo.Id);
        deletedTodoGroupTodo.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenTodoGroupTodoDoesNotExist()
    {
        // Arrange
        var nonExistingId = 999;

        // Act
        Func<Task> action = async () => await _todoGroupTodoCrud.DeleteAsync(nonExistingId, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoGroupTodoMessage.TodoGroupTodoNotFound);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteAsync_ShouldThrowException_WhenIdIsInvalid(int invalidId)
    {
        // Act
        Func<Task> action = async () => await _todoGroupTodoCrud.DeleteAsync(invalidId, default);

        // Assert
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"{nameof(TodoGroupTodo.Id)} ('{invalidId}') must be a non-negative and non-zero value. (Parameter '{nameof(TodoGroupTodo.Id)}')\nActual value was {invalidId}.");
    }
}
