using FluentAssertions;
using HomeApp.DataAccess.enums;
using HomeApp.DataAccess.Models;
using HomeApp.DataAccess.Tests.Helper;
using HomeApp.DataAccess.Tests.Helper.CreateDummyData;
using Xunit;

namespace HomeApp.DataAccess.Tests.Cruds.Todos.Queries;

public class TodoReadTests : BaseTodoQueriesTest
{
    private readonly CreateDummyTodos _createDummyTodos;

    public TodoReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _createDummyTodos = new CreateDummyTodos(unitTestingApiFactory);

    [Fact]
    public async Task FindByIdAsync_ReturnsTodo_WhenExists()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Test Todo",
            Done = false,
            Priority = TodoPriority.Low,
            LastModified = DateTimeOffset.UtcNow.AddDays(1)
        };
        DbContext.Todos.Add(todo);
        await DbContext.SaveChangesAsync();

        // Act
        var result = await TodoQueries.FindByIdAsync(todo.Id, default);

        // Assert
        result.Should().BeEquivalentTo(todo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await TodoQueries.FindByIdAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task FindByIdAsync_ThrowsException_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await TodoQueries.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoMessage.TodoNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTodosForSpecificPerson()
    {
        // Arrange
        var todo1 = await _createDummyTodos.CreateOneDummyTodo();
        var personId = todo1.TodoPeople.First().PersonId;
        var todo2 = await _createDummyTodos.CreateOneDummyTodo(personId);

        // Act
        var result = await TodoQueries.GetAllAsync(personId, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var resultList = result.ToList();
        resultList[0].Id.Should().Be(todo1.Id);

        resultList[1].Id.Should().Be(todo2.Id);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoTodosForPersonExist()
    {
        // Arrange
        var personId = 999; // Nicht existierende Person

        // Act
        var result = await TodoQueries.GetAllAsync(personId, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_IncludesTodoAndTodoGroupTodo()
    {
        // Arrange
        var todo = await _createDummyTodos.CreateOneDummyTodoWithGroupAndReturnsTodo();
        var personId = todo.TodoPeople.First().PersonId;

        // Act
        var result = await TodoQueries.GetAllAsync(personId, default);

        // Assert
        result.Should().NotBeNullOrEmpty();
        var todoDto = result.First();

        todoDto.Should().NotBeNull();
        todoDto.TodoGroupId.Should().Be(todo.TodoGroupTodo.TodoGroupId);
    }
}
