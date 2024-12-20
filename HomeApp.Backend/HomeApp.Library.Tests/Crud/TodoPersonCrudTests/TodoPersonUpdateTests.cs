namespace HomeApp.Library.Tests.Crud.TodoPersonCrudTests;

public class TodoPersonUpdateTests : BaseTodoPersonCrudTest
{
    [Fact]
    public async Task UpdateAsync_UpdatesTodoPersonInContext()
    {
        // Arrange
        var todoPerson = new TodoPerson { PersonId = 1, TodoId = 1 };

        _context.TodoPeople.Add(todoPerson);
        await _context.SaveChangesAsync();

        var updatedTodoPerson = new TodoPerson { Id = todoPerson.Id, PersonId = 2, TodoId = 2 };

        // Act
        await _todoPersonCrud.UpdateAsync(updatedTodoPerson, default);

        // Assert
        var result = await _context.TodoPeople.FindAsync(todoPerson.Id);
        result.Should().NotBeNull();
        result.PersonId.Should().Be(updatedTodoPerson.PersonId);
        result.TodoId.Should().Be(updatedTodoPerson.TodoId);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoPersonIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _todoPersonCrud.UpdateAsync(null, default));

    [Fact]
    public async Task UpdateAsync_ThrowsException_WhenTodoPersonDoesNotExist()
    {
        // Arrange
        var nonExistingTodoPerson = new TodoPerson
        {
            Id = 999, // Nicht existierende ID
            PersonId = 1,
            TodoId = 1
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _todoPersonCrud.UpdateAsync(nonExistingTodoPerson, default));
    }
}
