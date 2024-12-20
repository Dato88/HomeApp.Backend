namespace HomeApp.Library.Tests.Crud.TodoGroupCrudTests;

public class BaseTodoGroupCrudTest : BaseTest
{
    protected readonly TodoGroupCrud _todoGroupCrud;

    public BaseTodoGroupCrudTest() => _todoGroupCrud = new TodoGroupCrud(_context);
}
