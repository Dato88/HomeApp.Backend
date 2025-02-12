using HomeApp.DataAccess.Cruds.Interfaces.People;
using HomeApp.Library.Facades;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Tests.Facades;

public class BasePersonFacadeTest : BaseTest
{
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessor;
    protected readonly Mock<ILogger<PersonFacade>> ILogger;
    protected readonly Mock<IPersonCommands> PersonCommandsMock;
    protected readonly PersonFacade PersonFacade;

    public BasePersonFacadeTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        HttpContextAccessor = new Mock<IHttpContextAccessor>();
        HttpContextAccessor.DefaultValue = DefaultValue.Mock;
        HttpContextAccessor.SetupAllProperties();

        PersonCommandsMock = new Mock<IPersonCommands>();
        PersonCommandsMock.DefaultValue = DefaultValue.Mock;
        PersonCommandsMock.SetupAllProperties();

        ILogger = new Mock<ILogger<PersonFacade>>();
        ILogger.DefaultValue = DefaultValue.Mock;
        ILogger.SetupAllProperties();

        PersonFacade = new PersonFacade(DbContext, PersonCommandsMock.Object,
            HttpContextAccessor.Object,
            ILogger.Object);
    }
}
