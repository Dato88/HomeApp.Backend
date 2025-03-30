namespace HomeApp.Library.Tests.Crud.TodoPersonCrudTests;

public class TodoPersonReadTests : BaseTodoPersonCrudTest
{
    // public TodoPersonReadTests(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) { }
    //
    // [Fact]
    // public async Task FindByIdAsync_ReturnsTodoPerson_WhenExists()
    // {
    //     // Arrange
    //     TodoPerson todoPerson = new() { TodoId = 1, PersonId = 1 };
    //
    //     DbContext.TodoPeople.Add(todoPerson);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     var result = await _todoPersonCrud.FindByIdAsync(todoPerson.Id, default);
    //
    //     // Assert
    //     result.Should().BeEquivalentTo(todoPerson);
    // }
    //
    // [Theory]
    // [InlineData(0)]
    // [InlineData(-3)]
    // public async Task FindByIdAsync_ThrowsException_WhenIdIsInvalid(int invalidId)
    // {
    //     // Act & Assert
    //     Func<Task> action = async () => await _todoPersonCrud.FindByIdAsync(invalidId, default);
    //
    //     await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
    //         .WithMessage(
    //             $"{nameof(TodoPerson.Id)} ('{invalidId}') must be a non-negative and non-zero value. (Parameter '{nameof(TodoPerson.Id)}')\nActual value was {invalidId}.");
    // }
    //
    // [Fact]
    // public async Task FindByIdAsync_ThrowsException_WhenTodoPersonDoesNotExist()
    // {
    //     // Arrange
    //     var nonExistingId = 999;
    //
    //     // Act
    //     Func<Task> action = async () => await _todoPersonCrud.FindByIdAsync(nonExistingId, default);
    //
    //     // Assert
    //     await action.Should().ThrowAsync<InvalidOperationException>()
    //         .WithMessage(TodoPersonMessage.TodoPersonNotFound);
    // }
    //
    // [Fact]
    // public async Task GetAllAsync_ReturnsAllTodoPeople()
    // {
    //     // Arrange
    //     List<TodoPerson> todoPeople = new()
    //     {
    //         new TodoPerson { TodoId = 1, PersonId = 1 }, new TodoPerson { TodoId = 2, PersonId = 3 }
    //     };
    //
    //     DbContext.TodoPeople.AddRange(todoPeople);
    //     await DbContext.SaveChangesAsync();
    //
    //     // Act
    //     var result = await _todoPersonCrud.GetAllAsync(default);
    //
    //     // Assert
    //     result.Should().BeEquivalentTo(todoPeople);
    // }
    //
    // [Fact]
    // public async Task GetAllAsync_ReturnsEmptyList_WhenNoTodoPeopleExist()
    // {
    //     // Act
    //     var result = await _todoPersonCrud.GetAllAsync(default);
    //
    //     // Assert
    //     result.Should().BeEmpty();
    // }
}
