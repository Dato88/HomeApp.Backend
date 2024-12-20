namespace HomeApp.Library.Tests.Crud.TodoGroupCrudTests;

public class TodoGroupReadTests : BaseTodoGroupCrudTest
{
    [Fact]
    public async Task FindByIdAsync_ReturnsTodoGroup_WhenExists()
    {
        // Arrange
        TodoGroup todoGroup = new() { Name = "Test Todo Group" };
        _context.TodoGroups.Add(todoGroup);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoGroupCrud.FindByIdAsync(todoGroup.Id, default);

        // Assert
        result.Should().BeEquivalentTo(todoGroup);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsInvalid(int invalidId)
    {
        // Act & Assert
        Func<Task> action = async () => await _todoGroupCrud.FindByIdAsync(invalidId, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"{nameof(TodoGroup.Id)} ('{invalidId}') must be a non-negative and non-zero value. (Parameter '{nameof(TodoGroup.Id)}')\nActual value was {invalidId}.");
    }

    [Fact]
    public async Task FindByIdAsync_ThrowsException_WhenTodoGroupDoesNotExist()
    {
        // Arrange
        var nonExistingGroupId = 999;

        // Act
        Func<Task> action = async () => await _todoGroupCrud.FindByIdAsync(nonExistingGroupId, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoGroupMessage.TodoGroupNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllTodoGroups()
    {
        // Arrange
        List<TodoGroup> todoGroups = new() { new TodoGroup { Name = "Group 1" }, new TodoGroup { Name = "Group 2" } };

        _context.TodoGroups.AddRange(todoGroups);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoGroupCrud.GetAllAsync(default);

        // Assert
        result.Should().BeEquivalentTo(todoGroups);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoTodoGroupsExist()
    {
        // Act
        var result = await _todoGroupCrud.GetAllAsync(default);

        // Assert
        result.Should().BeEmpty();
    }
}
