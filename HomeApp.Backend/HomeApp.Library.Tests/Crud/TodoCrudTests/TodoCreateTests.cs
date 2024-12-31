using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Tests.Crud.TodoCrudTests;

public class TodoCreateTests : BaseTodoTest
{
    [Fact]
    public async Task CreateAsync_AddsTodoToContext()
    {
        // Arrange
        Todo todo = new()
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Normal, LastModified = DateTime.Now.AddDays(1)
        };

        // Act
        CancellationToken cancellationToken = new();
        await _todoCrud.CreateAsync(todo, cancellationToken);

        // Assert
        Assert.Contains(todo, _context.Todos);
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenTodoIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _todoCrud.CreateAsync(null, default));
}
