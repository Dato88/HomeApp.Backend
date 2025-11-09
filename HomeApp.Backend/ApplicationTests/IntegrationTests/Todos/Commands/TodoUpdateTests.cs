using Domain.Entities.Todos;
using Domain.Entities.Todos.Enums;

namespace ApplicationTests.IntegrationTests.Todos.Commands;

public class TodoUpdateTests : BaseTodoCommandsTest
{
    public TodoUpdateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }

    [Fact]
    public async Task UpdateAsync_UpdatesTodoInContext()
    {
        // Arrange
        var initialLastModified = DateTime.UtcNow.AddDays(-7);

        var todo = await TodosDataSeeder.CreateOneDummyTodoWithPersonId(null, initialLastModified);

        var updatedTodo = new Todo
        {
            TodoId = todo.TodoId,
            Title = "Updated Todo",
            Done = true,
            Priority = TodoPriority.High,
            UpdatedAt = DateTime.UtcNow,
            UpdatedById = 1000
        };

        // Act
        var result = await TodoCommands.UpdateAsync(updatedTodo, default);

        // Assert
        result.IsSuccess.Should().BeTrue();

        var dbTodo = await DbContext.Todos.FindAsync(todo.TodoId);
        dbTodo.Should().NotBeNull();
        dbTodo!.Title.Should().Be(updatedTodo.Title);
        dbTodo.Done.Should().Be(updatedTodo.Done);
        dbTodo.Priority.Should().Be(updatedTodo.Priority);
        dbTodo.UpdatedAt.Should().BeAfter(initialLastModified);
        dbTodo.UpdatedById.Should().Be(updatedTodo.UpdatedById);
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenUpdatedAtIsNull()
    {
        // Act
        var result = await TodoCommands.UpdateAsync(new(), default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(TodoErrors.UpdateFailedWithMessage("Todo.UpdatedAt should not be null"));
        result.Error.Description.Should().Contain("null");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task UpdateAsync_Fails_WhenUpdatedByIdIsZeroOrBelow(int? personId)
    {
        // Assert
        var newTodo = new Todo() { UpdatedAt = DateTime.UtcNow, UpdatedById = personId };

        // Act
        var result = await TodoCommands.UpdateAsync(newTodo, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should()
            .BeEquivalentTo(
                TodoErrors.UpdateFailedWithMessage("Todo.UpdatedById should not be null, 0 or lower than 0"));
        result.Error.Description.Should().Contain("null");
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoPriorityIsInvalid()
    {
        // Arrange
        var todo = await TodosDataSeeder.CreateOneDummyTodoWithPersonId();

        var invalidTodo = new Todo
        {
            TodoId = todo.TodoId,
            Title = "Test Todo",
            Done = false,
            Priority = (TodoPriority)(-1), // Invalid priority
            UpdatedAt = DateTime.UtcNow.AddDays(2),
            UpdatedById = 1
        };

        // Act
        var result = await TodoCommands.UpdateAsync(invalidTodo, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(TodoErrors.UpdateFailedWithMessage("Priority is invalid"));
    }

    [Fact]
    public async Task UpdateAsync_Fails_WhenTodoDoesNotExist()
    {
        // Arrange
        var todo = new Todo
        {
            TodoId = 999,
            Title = "Non-existing Todo",
            Done = false,
            Priority = TodoPriority.Low,
            UpdatedAt = DateTime.UtcNow.AddDays(1),
            UpdatedById = 1
        };

        // Act
        var result = await TodoCommands.UpdateAsync(todo, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeEquivalentTo(TodoErrors.UpdateFailed(todo.TodoId));
        result.Error.Description.Should().Contain("999");
    }
}
