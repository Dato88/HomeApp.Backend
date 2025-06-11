using SharedKernel.ValueObjects;

namespace ApplicationTests.IntegrationTests.Todos.Queries;

public class TodoReadTests : BaseTodoQueriesTest
{
    public TodoReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task FindByIdAsync_ReturnsTodo_WhenExists()
    {
        // Arrange
        var todo = await TodosDataSeeder.CreateOneDummyTodoWithPersonId();

        // Act
        var result = await TodoQueries.FindByIdAsync(todo.TodoId, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(todo,
            options => options
                .Excluding(t => t.CreatedAt)
                .Excluding(t => t.TodoGroupTodo)
                .Excluding(t => t.TodoPeople)
                .Excluding(t => t.LastModified));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ReturnsFailure_WhenIdIsInvalid(int id)
    {
        // Act
        var todoId = new TodoId(id);
        var result = await TodoQueries.FindByIdAsync(todoId, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Select(c => c.Should().Be("Todo.NotFoundById"));
        result.Errors.Select(c => c.Description.Should().Contain(todoId.ToString()));
    }

    [Fact]
    public async Task FindByIdAsync_ReturnsFailure_WhenTodoDoesNotExist()
    {
        // Act
        var result = await TodoQueries.FindByIdAsync(new TodoId(999), default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Select(c => c.Should().Be("Todo.NotFoundById"));
        result.Errors.Select(c => c.Description.Should().Contain("999"));
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTodosForSpecificPerson()
    {
        // Arrange
        var todo1 = await TodosDataSeeder.CreateOneDummyTodoWithPersonId();
        var personId = todo1.TodoPeople.First().PersonId;
        var todo2 = await TodosDataSeeder.CreateOneDummyTodoWithPersonId(personId);

        // Act
        var result = await TodoQueries.GetAllAsync(personId, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);

        var resultList = result.Value.ToList();
        resultList[0].TodoId.Should().Be(todo1.TodoId);
        resultList[1].TodoId.Should().Be(todo2.TodoId);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsFailure_WhenNoTodosForPersonExist()
    {
        // Arrange
        var userId = new PersonId(999);

        // Act
        var result = await TodoQueries.GetAllAsync(userId, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Select(c => c.Should().Be("Todo.NotFoundAll"));
        result.Errors.Select(c => c.Description.Should().Contain("999"));
    }

    [Fact]
    public async Task GetAllAsync_IncludesTodoAndTodoGroupTodo()
    {
        // Arrange
        var todo = await TodosDataSeeder.CreateOneDummyTodoWithGroupAndReturnsTodo();
        var personId = todo.TodoPeople.First().PersonId;

        // Act
        var result = await TodoQueries.GetAllAsync(personId, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNullOrEmpty();

        var first = result.Value.First();
        first.TodoGroupTodo.Should().NotBeNull();
        first.TodoGroupTodo.TodoGroupId.Should().Be(todo.TodoGroupTodo.TodoGroupId);
    }
}
