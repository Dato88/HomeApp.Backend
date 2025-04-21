namespace HomeApp.Library.Tests.Crud.TodoGroupCrudTests;

public class TodoGroupDeleteTests : BaseTodoGroupCrudTest
{
    // public TodoGroupDeleteTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task DeleteAsync_ShouldDeleteTodoGroup_WhenTodoGroupExists()
    // {
    //     // Arrange
    //     TodoGroup todoGroup = new() { Name = "Test Todo Group" };
    //
    //     DbContext.TodoGroups.Add(todoGroup);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     var result = await _todoGroupCrud.DeleteAsync(todoGroup.Id, default);
    //
    //     // Assert
    //     result.Should().BeTrue();
    //     var deletedGroup = await DbContext.TodoGroups.FindAsync(todoGroup.Id);
    //     deletedGroup.Should().BeNull();
    // }
    //
    // [Fact]
    // public async Task DeleteAsync_ShouldThrowException_WhenTodoGroupDoesNotExist()
    // {
    //     // Arrange
    //     var nonExistingGroupId = 999;
    //
    //     // Act
    //     Func<Task> action = async () => await _todoGroupCrud.DeleteAsync(nonExistingGroupId, default);
    //
    //     // Assert
    //     await action.Should().ThrowAsync<InvalidOperationException>()
    //         .WithMessage(TodoGroupMessage.TodoGroupNotFound);
    // }
    //
    // [Theory]
    // [InlineData(0)]
    // [InlineData(-1)]
    // public async Task DeleteAsync_ShouldThrowException_WhenIdIsInvalid(int invalidId)
    // {
    //     // Act
    //     Func<Task> action = async () => await _todoGroupCrud.DeleteAsync(invalidId, default);
    //
    //     // Assert
    //     await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
    //         .WithMessage(
    //             $"{nameof(TodoGroup.Id)} ('{invalidId}') must be a non-negative and non-zero value. (Parameter '{nameof(TodoGroup.Id)}')\nActual value was {invalidId}.");
    // }
}
