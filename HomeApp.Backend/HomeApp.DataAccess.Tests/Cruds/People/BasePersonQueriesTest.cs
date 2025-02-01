using HomeApp.DataAccess.Cruds.People;
using HomeApp.DataAccess.Tests.Helper;

namespace HomeApp.DataAccess.Tests.Cruds.People;

public class BasePersonQueriesTest : BaseTest
{
    protected readonly PersonQueries PersonQueries;


    public BasePersonQueriesTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory) =>
        PersonQueries = new PersonQueries(DbContext);
}
