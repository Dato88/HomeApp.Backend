namespace HomeApp.Library.Tests.Crud.TodoCrudTests;

public class BaseTodoTest : BaseTest
{
    protected readonly TodoCrud _todoCrud;

    public BaseTodoTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        _todoCrud = new TodoCrud(DbContext);
}
