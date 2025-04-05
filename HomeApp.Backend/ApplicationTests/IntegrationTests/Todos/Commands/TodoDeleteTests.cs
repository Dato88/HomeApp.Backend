namespace ApplicationTests.IntegrationTests.Todos.Commands;

public class TodoDeleteTests : BaseTodoCommandsTest
{
    public TodoDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task DeleteAsync_ReturnsSuccess_WhenTodoExists()
    {
        // Arrange
        var todo = await CreateDummyTodos.CreateOneDummyTodoWithPersonId();

        // Act
        var result = await TodoCommands.DeleteAsync(todo.Id, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.DoesNotContain(todo, DbContext.Todos);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task DeleteAsync_Fails_WhenIdIsInvalid(int id)
    {
        // Act
        var result = await TodoCommands.DeleteAsync(id, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Todo.DeleteFailedWithMessage", result.Error.Code);
        Assert.Contains("Invalid ID", result.Error.Description);
    }

    [Fact]
    public async Task DeleteAsync_Fails_WhenTodoDoesNotExist()
    {
        // Arrange
        const int nonExistentId = 999;

        // Act
        var result = await TodoCommands.DeleteAsync(nonExistentId, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Todo.DeleteFailed", result.Error.Code);
        Assert.Contains(nonExistentId.ToString(), result.Error.Description);
    }
}
