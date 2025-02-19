using HomeApp.DataAccess;

namespace HomeApp.Library.Tests.Common.Todos.Queries;

public class TodoReadTests : BaseTodoQueriesTest
{
    public TodoReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task FindByIdAsync_ReturnsTodo_WhenExists()
    {
        // Arrange
        var todo = await CreateDummyTodos.CreateOneDummyTodoWithPersonId();

        // Act
        var result = await TodoQueries.FindByIdAsync(todo.Id, default);

        // Assert
        result.Should().BeEquivalentTo(todo,
            options => options.Excluding(t => t.TodoGroupTodo).Excluding(t => t.TodoPeople)
                .Excluding(t => t.LastModified));
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
        var todo1 = await CreateDummyTodos.CreateOneDummyTodoWithPersonId();
        var personId = todo1.TodoPeople.First().PersonId;
        var todo2 = await CreateDummyTodos.CreateOneDummyTodoWithPersonId(personId);

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
        var todo = await CreateDummyTodos.CreateOneDummyTodoWithGroupAndReturnsTodo();
        var personId = todo.TodoPeople.First().PersonId;

        // Act
        var result = await TodoQueries.GetAllAsync(personId, default);

        // Assert
        result.Should().NotBeNullOrEmpty();
        var todoDto = result.First();

        todoDto.Should().NotBeNull();
        todoDto.TodoGroupTodo.TodoGroupId.Should().Be(todo.TodoGroupTodo.TodoGroupId);
    }
}
