using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Tests.Crud.TodoCrudTests;

public class TodoCrudReadTests : BaseTodoTest
{
    [Fact]
    public async Task FindByIdAsync_ReturnsTodo_WhenExists()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Low, ExecutionDate = DateTime.Now.AddDays(1)
        };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoCrud.FindByIdAsync(todo.Id, default);

        // Assert
        result.Should().BeEquivalentTo(todo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _todoCrud.FindByIdAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task FindByIdAsync_ThrowsException_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await _todoCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Todo not found.");
    }
}
