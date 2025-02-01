using FluentAssertions;
using HomeApp.DataAccess.Tests.Helper;
using HomeApp.DataAccess.Tests.Helper.CreateDummyData;
using Xunit;

namespace HomeApp.DataAccess.Tests.Cruds.Todos.Commands;

public class TodoDeleteTests : BaseTodoCommandsTest
{
    private readonly CreateDummyTodos _createDummyTodos;

    public TodoDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _createDummyTodos = new CreateDummyTodos(unitTestingApiFactory);

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenTodoExists()
    {
        // Arrange
        var todo = await _createDummyTodos.CreateOneDummyTodo();

        // Act
        CancellationToken cancellationToken = new();
        var result = await TodoCommands.DeleteAsync(todo.Id, cancellationToken);

        // Assert
        result.Should().BeTrue();
        Assert.DoesNotContain(todo, DbContext.Todos);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task DeleteAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await TodoCommands.DeleteAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task DeleteAsync_ThrowsException_WhenTodoNotFound()
    {
        // Act & Assert
        Func<Task> action = async () => await TodoCommands.DeleteAsync(999, default);

        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoMessage.TodoNotFound);
    }
}
