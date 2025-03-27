using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Tests.Common.People;

public class BaseCommonPersonTest : BaseTest
{
    protected readonly Mock<ILogger<CommonPersonCommands>> CommandsILogger;
    protected readonly CommonPersonCommands CommonPersonCommands;
    protected readonly CommonPersonQueries CommonPersonQueries;
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessor;
    protected readonly Mock<IPersonValidation> PersonValidationMock;
    protected readonly Mock<ILogger<CommonPersonQueries>> QuerriesILogger;

    public BaseCommonPersonTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        HttpContextAccessor = new Mock<IHttpContextAccessor>();
        HttpContextAccessor.DefaultValue = DefaultValue.Mock;
        HttpContextAccessor.SetupAllProperties();

        QuerriesILogger = new Mock<ILogger<CommonPersonQueries>>();
        QuerriesILogger.DefaultValue = DefaultValue.Mock;
        QuerriesILogger.SetupAllProperties();

        CommandsILogger = new Mock<ILogger<CommonPersonCommands>>();
        CommandsILogger.DefaultValue = DefaultValue.Mock;
        CommandsILogger.SetupAllProperties();

        PersonValidationMock = new Mock<IPersonValidation>();
        PersonValidationMock.DefaultValue = DefaultValue.Mock;
        PersonValidationMock.SetupAllProperties();

        CommonPersonCommands = new CommonPersonCommands(DbContext,
            PersonValidationMock.Object,
            CommandsILogger.Object);

        CommonPersonQueries = new CommonPersonQueries(DbContext,
            HttpContextAccessor.Object,
            QuerriesILogger.Object);
    }
}
