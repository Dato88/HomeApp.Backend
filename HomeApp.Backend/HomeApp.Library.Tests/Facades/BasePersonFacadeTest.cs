using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Facades;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Tests.Facades;

public class BasePersonFacadeTest : BaseTest
{
    protected readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    protected readonly Mock<ILogger<PersonFacade>> _iLogger;
    protected readonly Mock<IPersonCrud> _personCrudMock;
    protected readonly PersonFacade _personFacade;

    public BasePersonFacadeTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
        _httpContextAccessor.DefaultValue = DefaultValue.Mock;
        _httpContextAccessor.SetupAllProperties();

        _personCrudMock = new Mock<IPersonCrud>();
        _personCrudMock.DefaultValue = DefaultValue.Mock;
        _personCrudMock.SetupAllProperties();

        _iLogger = new Mock<ILogger<PersonFacade>>();
        _iLogger.DefaultValue = DefaultValue.Mock;
        _iLogger.SetupAllProperties();

        _personFacade = new PersonFacade(_personCrudMock.Object, _httpContextAccessor.Object, _iLogger.Object);
    }
}
