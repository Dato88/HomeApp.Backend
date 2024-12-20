namespace HomeApp.Library.Tests.Crud.TodoPersonCrudTests;

public class TodoPersonDeleteTests : BaseTodoPersonCrudTest
{
    [Fact]
    public async Task DeleteAsync_ShouldDeleteTodoPerson_WhenTodoPersonExists()
    {
        // Arrange
        TodoPerson todoPerson = new() { TodoId = 1, PersonId = 1 };

        _context.TodoPeople.Add(todoPerson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoPersonCrud.DeleteAsync(todoPerson.Id, default);

        // Assert
        result.Should().BeTrue();
        var deletedTodoPerson = await _context.TodoPeople.FindAsync(todoPerson.Id);
        deletedTodoPerson.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldThrowException_WhenTodoPersonDoesNotExist()
    {
        // Arrange
        var nonExistingId = 999;

        // Act
        Func<Task> action = async () => await _todoPersonCrud.DeleteAsync(nonExistingId, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoPersonMessage.TodoPersonNotFound);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task DeleteAsync_ShouldThrowException_WhenIdIsInvalid(int invalidId)
    {
        // Act
        Func<Task> action = async () => await _todoPersonCrud.DeleteAsync(invalidId, default);

        // Assert
        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"{nameof(TodoPerson.Id)} ('{invalidId}') must be a non-negative and non-zero value. (Parameter '{nameof(TodoPerson.Id)}')\nActual value was {invalidId}.");
    }
}
