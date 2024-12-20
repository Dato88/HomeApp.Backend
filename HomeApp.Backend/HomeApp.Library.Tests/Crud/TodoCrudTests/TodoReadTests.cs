using HomeApp.DataAccess.enums;

namespace HomeApp.Library.Tests.Crud.TodoCrudTests;

public class TodoReadTests : BaseTodoTest
{
    [Fact]
    public async Task FindByIdAsync_ReturnsTodo_WhenExists()
    {
        // Arrange
        var todo = new Todo
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Low, ExecutionDate = DateTime.Now.AddDays(1)
        };
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoCrud.FindByIdAsync(todo.Id, default);

        // Assert
        result.Should().BeEquivalentTo(todo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public async Task FindByIdAsync_ThrowsException_WhenIdIsNullOrEmpty(int id)
    {
        // Act & Assert
        Func<Task> action = async () => await _todoCrud.FindByIdAsync(id, default);

        await action.Should().ThrowAsync<ArgumentOutOfRangeException>()
            .WithMessage(
                $"id ('{id}') must be a non-negative and non-zero value. (Parameter 'id')Actual value was {id}.");
    }

    [Fact]
    public async Task FindByIdAsync_ThrowsException_WhenNotExists()
    {
        // Assert
        Func<Task> action = async () => await _todoCrud.FindByIdAsync(999, default);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage(TodoMessage.TodoNotFound);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTodosForSpecificPerson()
    {
        // Arrange
        var personId = 1;

        var todo1 = new Todo
        {
            Name = "Test Todo 2",
            Done = false,
            Priority = TodoPriority.Normal,
            ExecutionDate = DateTime.Now.AddDays(1)
        };
        var todo2 = new Todo
        {
            Name = "Test Todo 2",
            Done = false,
            Priority = TodoPriority.Normal,
            ExecutionDate = DateTime.Now.AddDays(1)
        };

        var todoPerson1 = new TodoPerson { PersonId = personId, Todo = todo1 };
        var todoPerson2 = new TodoPerson { PersonId = personId, Todo = todo2 };

        _context.TodoPeople.AddRange(todoPerson1, todoPerson2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoCrud.GetAllAsync(personId, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var resultList = result.ToList();
        resultList[0].Id.Should().Be(todo1.Id);

        resultList[1].Id.Should().Be(todo2.Id);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoTodosForPersonExist()
    {
        // Arrange
        var personId = 999; // Nicht existierende Person

        // Act
        var result = await _todoCrud.GetAllAsync(personId, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_IncludesTodoAndTodoGroupTodo()
    {
        // Arrange
        var personId = 1;

        var todoGroupTodo = new TodoGroupTodo { TodoGroupId = 1 };
        var todo = new Todo
        {
            Name = "Test Todo 2",
            Done = false,
            Priority = TodoPriority.Normal,
            ExecutionDate = DateTime.Now.AddDays(1),
            TodoGroupTodo = todoGroupTodo
        };
        var todoPerson = new TodoPerson { PersonId = personId, Todo = todo };

        _context.TodoPeople.Add(todoPerson);
        await _context.SaveChangesAsync();

        // Act
        var result = await _todoCrud.GetAllAsync(personId, default);

        // Assert
        result.Should().NotBeNullOrEmpty();
        var todoDto = result.First();

        todoDto.Should().NotBeNull();
        todoDto.TodoGroupId.Should().Be(todoGroupTodo.TodoGroupId);
    }
}
