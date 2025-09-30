using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Features.People.Validations;
using Infrastructure.Features.People.Commands;
using Infrastructure.Features.People.Queries;
using Microsoft.AspNetCore.Http;

namespace ApplicationTests.IntegrationTests.People;

public class BaseCommonPersonTest : BaseTest
{
    protected readonly Mock<IAppLogger<PersonCommands>> CommandsILogger;
    protected readonly PersonCommands CommonPersonCommands;
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessor;
    protected readonly PersonQueries PersonQueries;
    protected readonly Mock<IPersonValidation> PersonValidationMock;
    protected readonly Mock<IAppLogger<PersonQueries>> QuerriesILogger;
    protected readonly Mock<IUserContext> UserContext;

    public BaseCommonPersonTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        HttpContextAccessor = new Mock<IHttpContextAccessor>();
        HttpContextAccessor.DefaultValue = DefaultValue.Mock;
        HttpContextAccessor.SetupAllProperties();

        QuerriesILogger = new Mock<IAppLogger<PersonQueries>>();
        QuerriesILogger.DefaultValue = DefaultValue.Mock;
        QuerriesILogger.SetupAllProperties();

        CommandsILogger = new Mock<IAppLogger<PersonCommands>>();
        CommandsILogger.DefaultValue = DefaultValue.Mock;
        CommandsILogger.SetupAllProperties();

        PersonValidationMock = new Mock<IPersonValidation>();
        PersonValidationMock.DefaultValue = DefaultValue.Mock;
        PersonValidationMock.SetupAllProperties();

        UserContext = new Mock<IUserContext>();
        UserContext.DefaultValue = DefaultValue.Mock;
        UserContext.SetupAllProperties();

        CommonPersonCommands = new PersonCommands(DbContext,
            PersonValidationMock.Object,
            CommandsILogger.Object);

        PersonQueries = new PersonQueries(DbContext, UserContext.Object,
            QuerriesILogger.Object);
    }
}
