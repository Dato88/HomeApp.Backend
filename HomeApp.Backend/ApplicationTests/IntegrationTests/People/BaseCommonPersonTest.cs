using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Common.People;
using Application.Common.People.Validations.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ApplicationTests.IntegrationTests.People;

public class BaseCommonPersonTest : BaseTest
{
    protected readonly Mock<IAppLogger<CommonPersonCommands>> CommandsILogger;
    protected readonly CommonPersonCommands CommonPersonCommands;
    protected readonly CommonPersonQueries CommonPersonQueries;
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessor;
    protected readonly Mock<IPersonValidation> PersonValidationMock;
    protected readonly Mock<IAppLogger<CommonPersonQueries>> QuerriesILogger;
    protected readonly Mock<IUserContext> UserContext;

    public BaseCommonPersonTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        HttpContextAccessor = new Mock<IHttpContextAccessor>();
        HttpContextAccessor.DefaultValue = DefaultValue.Mock;
        HttpContextAccessor.SetupAllProperties();

        QuerriesILogger = new Mock<IAppLogger<CommonPersonQueries>>();
        QuerriesILogger.DefaultValue = DefaultValue.Mock;
        QuerriesILogger.SetupAllProperties();

        CommandsILogger = new Mock<IAppLogger<CommonPersonCommands>>();
        CommandsILogger.DefaultValue = DefaultValue.Mock;
        CommandsILogger.SetupAllProperties();

        PersonValidationMock = new Mock<IPersonValidation>();
        PersonValidationMock.DefaultValue = DefaultValue.Mock;
        PersonValidationMock.SetupAllProperties();

        UserContext = new Mock<IUserContext>();
        UserContext.DefaultValue = DefaultValue.Mock;
        UserContext.SetupAllProperties();

        CommonPersonCommands = new CommonPersonCommands(DbContext,
            PersonValidationMock.Object,
            CommandsILogger.Object);

        CommonPersonQueries = new CommonPersonQueries(DbContext, UserContext.Object,
            QuerriesILogger.Object);
    }
}
