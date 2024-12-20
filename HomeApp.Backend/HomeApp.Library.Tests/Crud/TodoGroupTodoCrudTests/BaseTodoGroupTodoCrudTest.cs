namespace HomeApp.Library.Tests.Crud.TodoGroupTodoCrudTests;

public class BaseTodoGroupTodoCrudTest : BaseTest
{
    protected readonly TodoGroupTodoCrud _todoGroupTodoCrud;

    public BaseTodoGroupTodoCrudTest() => _todoGroupTodoCrud = new TodoGroupTodoCrud(_context);
}
