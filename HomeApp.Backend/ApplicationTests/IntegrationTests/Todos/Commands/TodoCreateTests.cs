using Domain.Entities.Todos;

namespace ApplicationTests.IntegrationTests.Todos.Commands;

public class TodoCreateTests : BaseTodoCommandsTest
{
    public TodoCreateTests(UnitTestingApiFactory unitTestingApiFactory)
        : base(unitTestingApiFactory)
    {
    }

    [Fact]
    public async Task CreateAsync_AddsTodoToContext()
    {
        // Arrange
        var person = await PeopleDataSeeder.SeedPersonAsync();
        var todo = await TodosDataSeeder.GenereateDummyTodo(person.PersonId);

        // Act
        var result = await TodoCommands.CreateAsync(todo, default);

        // Assert
        result.IsSuccess.Should().BeTrue("a valid todo should be created");
    }

    [Fact]
    public async Task CreateAsync_Should_Also_Create_TodoGroupTodo()
    {
        // Arrange
        var person = await PeopleDataSeeder.SeedPersonAsync();

        var todoGroup = new TodoGroup { Name = "TestGroup" };
        DbContext.TodoGroups.Add(todoGroup);
        await DbContext.SaveChangesAsync();

        var newTodo = await TodosDataSeeder.GenereateDummyTodo(person.PersonId);
        newTodo.TodoGroupTodo = new TodoGroupTodo { TodoGroupId = todoGroup.TodoGroupId };

        // Act
        var result = await TodoCommands.CreateAsync(newTodo, default);

        // Assert
        result.IsSuccess.Should().BeTrue("todo with group should be created");
    }

    [Fact]
    public async Task CreateAsync_ShouldNotCreateWithoutPersonId()
    {
        // Arrange
        var todo = await TodosDataSeeder.GenereateDummyTodo();

        // Act
        var result = await TodoCommands.CreateAsync(todo, default);

        // Assert
        result.IsFailure.Should().BeTrue("no valid personId was provided");
        result.Errors.Select(c => c.Should().Be("Todo.CreateFailedWithMessage"));
        result.Errors.Select(c =>
            c.Description.Should().Contain("person", "the error should describe the missing personId"));
    }

    [Fact]
    public async Task CreateAsync_Fails_WhenTodoIsNull()
    {
        // Act
        var result = await TodoCommands.CreateAsync(null, default);

        // Assert
        result.IsFailure.Should().BeTrue("todo is null and should not be created");
        result.Errors.Select(c => c.Should().Be("Todo.CreateFailedWithMessage"));
        result.Errors.Select(c =>
            c.Description.Should().Contain("null", "null values should be rejected"));
    }
}
