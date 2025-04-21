using Domain.Entities.Todos;

namespace ApplicationTests.IntegrationTests.Todos.Commands;

public class TodoDeleteTests : BaseTodoCommandsTest
{
    public TodoDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task DeleteAsync_ReturnsSuccess_WhenTodoExists()
    {
        // Arrange
        var todo = await TodosDataSeeder.CreateOneDummyTodoWithPersonId();

        // Act
        var result = await TodoCommands.DeleteAsync(todo.Id, default);

        // Assert
        result.IsSuccess.Should().BeTrue("the todo exists and should be deleted");
        DbContext.Todos.Should().NotContain(todo, "the todo should have been removed from the context");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task DeleteAsync_Fails_WhenIdIsInvalid(int id)
    {
        // Act
        var result = await TodoCommands.DeleteAsync(id, default);

        // Assert
        result.IsFailure.Should().BeTrue("ID is invalid and deletion should fail");
        result.Error.Code.Should().Be(TodoErrors.DeleteFailed(id).Code);
    }

    [Fact]
    public async Task DeleteAsync_Fails_WhenTodoDoesNotExist()
    {
        // Arrange
        const int nonExistentId = 999;

        // Act
        var result = await TodoCommands.DeleteAsync(nonExistentId, default);

        // Assert
        result.IsFailure.Should().BeTrue("the todo does not exist");
        result.Error.Code.Should().Be(TodoErrors.DeleteFailed(nonExistentId).Code);
        result.Error.Description.Should()
            .Contain(nonExistentId.ToString(), "the error message should mention the missing ID");
    }
}
