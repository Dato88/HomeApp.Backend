﻿namespace HomeApp.Library.Tests.Crud.TodoGroupCrudTests;

public class TodoGroupCreateTests : BaseTodoGroupCrudTest
{
    // public TodoGroupCreateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task CreateAsync_AddsTodoGroupToContext()
    // {
    //     // Arrange
    //     TodoGroup todoGroup = new() { Name = "Test Todo Group" };
    //
    //     // Act
    //     CancellationToken cancellationToken = new();
    //     await _todoGroupCrud.CreateAsync(todoGroup, cancellationToken);
    //
    //     // Assert
    //     Assert.Contains(todoGroup, DbContext.TodoGroups);
    // }
    //
    // [Fact]
    // public async Task CreateAsync_ThrowsException_WhenTodoGroupIsNull() =>
    //     // Act & Assert
    //     await Assert.ThrowsAsync<ArgumentNullException>(async () =>
    //         await _todoGroupCrud.CreateAsync(null, default));
}
