﻿namespace HomeApp.Library.Tests.Crud.TodoPersonCrudTests;

public class TodoPersonCreateTests : BaseTodoPersonCrudTest
{
    // public TodoPersonCreateTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task CreateAsync_AddsTodoPersonToContext()
    // {
    //     // Arrange
    //     TodoPerson todoPerson = new() { TodoId = 1, PersonId = 1 };
    //
    //     // Act
    //     CancellationToken cancellationToken = new();
    //     await _todoPersonCrud.CreateAsync(todoPerson, cancellationToken);
    //
    //     // Assert
    //     Assert.Contains(todoPerson, DbContext.TodoPeople);
    // }
    //
    // [Fact]
    // public async Task CreateAsync_ThrowsException_WhenTodoPersonIsNull() =>
    //     // Act & Assert
    //     await Assert.ThrowsAsync<ArgumentNullException>(async () =>
    //         await _todoPersonCrud.CreateAsync(null, default));
}
