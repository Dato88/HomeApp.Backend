using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Tests.Crud.TodoCrudTests;

public class TodoDeleteTests : BaseTodoTest
{
    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenTodoExists()
    {
        // Arrange
        Todo todo = new()
        {
            Name = "Test Todo",
            Done = false,
            Priority = TodoPriority.Normal,
            ExecutionDate = DateTime.Now.AddDays(1)
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        // Act
        CancellationToken cancellationToken = new();
        var result = await _todoCrud.DeleteAsync(todo.Id, cancellationToken);

        // Assert
        result.Should().BeTrue();
        Assert.DoesNotContain(todo, _context.Todos);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task DeleteAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _todoCrud.DeleteAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenTodoNotFound()
    {
        // Act & Assert
        Func<Task> action = async () => await _todoCrud.DeleteAsync(999, default);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoMessage.TodoNotFound);
    }
}
