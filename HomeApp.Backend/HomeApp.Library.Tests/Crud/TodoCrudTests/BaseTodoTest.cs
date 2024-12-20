namespace HomeApp.Library.Tests.Crud.TodoCrudTests;

public class BaseTodoTest : BaseTest
{
    protected readonly TodoCrud _todoCrud;

    public BaseTodoTest() => _todoCrud = new TodoCrud(_context);
}
