﻿namespace HomeApp.Library.Tests.Crud.TodoGroupCrudTests;

public class TodoGroupUpdateTests : BaseTodoGroupCrudTest
{
    [Fact]
    public async Task UpdateAsync_UpdatesTodoGroupInContext()
    {
        // Arrange
        var todoGroup = new TodoGroup { Name = "Initial Todo Group" };

        _context.TodoGroups.Add(todoGroup);
        await _context.SaveChangesAsync();

        var updatedTodoGroup = new TodoGroup { Id = todoGroup.Id, Name = "Updated Todo Group" };

        // Act
        await _todoGroupCrud.UpdateAsync(updatedTodoGroup, default);

        // Assert
        var result = await _context.TodoGroups.FindAsync(todoGroup.Id);
        result.Should().NotBeNull();
        result.Name.Should().Be(updatedTodoGroup.Name);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoGroupIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _todoGroupCrud.UpdateAsync(null, default));

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoGroupDoesNotExist()
    {
        // Arrange
        var nonExistingTodoGroup = new TodoGroup
        {
            Id = 999, // Non-existing ID
            Name = "Non-existing Group"
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _todoGroupCrud.UpdateAsync(nonExistingTodoGroup, default));
    }
}