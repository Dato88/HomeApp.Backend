using Application.Abstractions.Authentication;
using Application.Abstractions.Logging;
using Application.Features.People.Validations;
using Infrastructure.Features.People.Commands;
using Infrastructure.Features.People.Queries;
using Infrastructure.Features.People.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace ApplicationTests.IntegrationTests.People;

public class BaseCommonPersonTest : BaseTest
{
    protected readonly Mock<IAppLogger<PersonCommands>> CommandsILogger;
    protected readonly PersonCommands CommonPersonCommands;
    protected readonly Mock<IHttpContextAccessor> HttpContextAccessor;
    protected readonly PersonQueries PersonQueries;
    protected readonly Mock<IPersonValidation> PersonValidationMock;
    protected readonly Mock<IAppLogger<PersonQueries>> QuerriesILogger;
    protected readonly Mock<IExecutionContextAccessor> ExecutionContextMock;
    protected readonly IPersonIdCache PersonIdCache;

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

        ExecutionContextMock = new Mock<IExecutionContextAccessor>();
        ExecutionContextMock.DefaultValue = DefaultValue.Mock;
        ExecutionContextMock.SetupAllProperties();

        PersonIdCache = new PersonIdCache(new MemoryCache(new MemoryCacheOptions()));

        CommonPersonCommands = new PersonCommands(DbContext,
            PersonValidationMock.Object,
            PersonIdCache,
            CommandsILogger.Object);

        PersonQueries = new PersonQueries(DbContext, ExecutionContextMock.Object,
            QuerriesILogger.Object);
    }
}
