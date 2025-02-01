using HomeApp.DataAccess.enums;
using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using HomeApp.DataAccess.Tests.Helper;
using HomeApp.DataAccess.Tests.Helper.CreateDummyData;

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

        Todo todo = new CreateToDoDto
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
    public async Task CreateAsync_ShouldNotCreateWithoutPersonId()
    {
        // Arrange
        Todo todo = new CreateToDoDto { Name = "Test Todo", Done = false, Priority = TodoPriority.Normal };

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
