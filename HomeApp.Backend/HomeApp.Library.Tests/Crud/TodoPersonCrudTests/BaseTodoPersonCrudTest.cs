namespace HomeApp.Library.Tests.Crud.TodoPersonCrudTests;

public class BaseTodoPersonCrudTest : BaseTest
{
    protected readonly TodoPersonCrud _todoPersonCrud;

    public BaseTodoPersonCrudTest() => _todoPersonCrud = new TodoPersonCrud(_context);
}
