using HomeApp.DataAccess.Enums;
using HomeApp.DataAccess.Tests.Helper;
using HomeApp.DataAccess.Tests.Helper.CreateDummyData;
using HomeApp.Library.Todos.Commands;

namespace HomeApp.DataAccess.Tests.Cruds.Todos.Commands;

public class TodoCreateTests : BaseTodoCommandsTest
{
    private readonly CreateDummyPeople _createDummyPeople;

    public TodoCreateTests(UnitTestingApiFactory unitTestingApiFactory) :
        base(unitTestingApiFactory) => _createDummyPeople = new CreateDummyPeople(unitTestingApiFactory);

    [Fact]
    public async Task CreateAsync_AddsTodoToContext()
    {
        // Arrange
        var dummyPerson = await _createDummyPeople.CreateOneDummyPerson();

        Todo todo = new CreateTodoCommand
        {
            Name = "Test Todo", Done = false, Priority = TodoPriority.Normal, PersonId = dummyPerson.Id
        };

        // Act
        CancellationToken cancellationToken = new();
        await TodoCommands.CreateAsync(todo, cancellationToken);

        // Assert
        Assert.Contains(todo, DbContext.Todos);
    }

    [Fact]
    public async Task CreateAsync_Should_Also_Create_TodoGroupTodo()
    {
        // Arrange
        var dummyPerson = await _createDummyPeople.CreateOneDummyPerson();

        var todoGroup = new TodoGroup { Name = "TestGroup" };
        DbContext.TodoGroups.Add(todoGroup);
        await DbContext.SaveChangesAsync();

        var newTodo = new CreateTodoCommand
        {
            Name = "Test Todo",
            Done = false,
            Priority = TodoPriority.Normal,
            PersonId = dummyPerson.Id,
            TodoGroupId = todoGroup.Id
        };

        // Act
        CancellationToken cancellationToken = new();
        var createdTodoId = await TodoCommands.CreateAsync(newTodo, cancellationToken);

        var createdTodoExists = DbContext.Todos.Any(x => x.Id == createdTodoId);

        // Assert
        Assert.True(createdTodoExists);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotCreateWithoutPersonId()
    {
        // Arrange
        Todo todo = new CreateTodoCommand { Name = "Test Todo", Done = false, Priority = TodoPriority.Normal };

        // Act
        CancellationToken cancellationToken = new();

        Func<Task> action = async () => await TodoCommands.CreateAsync(todo, cancellationToken);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Todo can`t be created without personId.");
    }

    [Fact]
    public async Task CreateAsync_ThrowsException_WhenTodoIsNull() =>
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await TodoCommands.CreateAsync(null, default));
}
