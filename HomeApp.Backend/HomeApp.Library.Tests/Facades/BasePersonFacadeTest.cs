using HomeApp.Library.Facades;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Tests.Facades;

public class BasePersonFacadeTest : BaseTest
{
    protected readonly Mock<ILogger<BudgetFacade>> _iLogger;

    protected readonly Mock<IPersonCrud> _personCrudMock;
    protected readonly PersonFacade _personFacade;

    public BasePersonFacadeTest()
    {
        _personCrudMock = new Mock<IPersonCrud>();
        _personCrudMock.DefaultValue = DefaultValue.Mock;
        _personCrudMock.SetupAllProperties();

        _iLogger = new Mock<ILogger<BudgetFacade>>();
        _iLogger.DefaultValue = DefaultValue.Mock;
        _iLogger.SetupAllProperties();

        _personFacade = new PersonFacade(_personCrudMock.Object, _iLogger.Object);
    }
}
