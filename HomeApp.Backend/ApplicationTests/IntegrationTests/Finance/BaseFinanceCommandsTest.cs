using Application.Abstractions.Authentication;
using ApplicationTests.IntegrationTests.TestData;
using Domain.ValueObjects;
using Infrastructure.Features.Finance.Commands;
using Infrastructure.Features.Finance.Queries;
using Infrastructure.Features.Finance.Services;

namespace ApplicationTests.IntegrationTests.Finance;

public class BaseFinanceCommandsTest : BaseTest
{
    protected readonly PeopleDataSeeder PeopleDataSeeder;
    protected readonly HouseholdDataSeeder HouseholdDataSeeder;
    protected readonly FinanceDataSeeder FinanceDataSeeder;
    protected readonly AccountCommands AccountCommands;
    protected readonly AccountQueries AccountQueries;
    protected readonly CategoryCommands CategoryCommands;
    protected readonly CategoryQueries CategoryQueries;
    protected readonly CategoryGroupCommands CategoryGroupCommands;
    protected readonly CategoryGroupQueries CategoryGroupQueries;
    protected readonly TransactionCommands TransactionCommands;
    protected readonly TransactionQueries TransactionQueries;
    protected readonly PaymentPartnerResolver PaymentPartnerResolver;
    protected readonly PaymentPartnerCommands PaymentPartnerCommands;
    protected readonly PaymentPartnerQueries PaymentPartnerQueries;
    protected readonly ReportQueries ReportQueries;

    protected BaseFinanceCommandsTest(UnitTestingApiFactory unitTestingApiFactory) : base(
        unitTestingApiFactory, BuildExecutionContextMock(unitTestingApiFactory).Result)
    {
        PeopleDataSeeder = new PeopleDataSeeder(unitTestingApiFactory);
        HouseholdDataSeeder = new HouseholdDataSeeder(unitTestingApiFactory);
        FinanceDataSeeder = new FinanceDataSeeder(unitTestingApiFactory);
        AccountCommands = new AccountCommands(DbContext, ExecutionContext);
        AccountQueries = new AccountQueries(DbContext, ExecutionContext);
        CategoryCommands = new CategoryCommands(DbContext, ExecutionContext);
        CategoryQueries = new CategoryQueries(DbContext, ExecutionContext);
        CategoryGroupCommands = new CategoryGroupCommands(DbContext, ExecutionContext);
        CategoryGroupQueries = new CategoryGroupQueries(DbContext, ExecutionContext);
        PaymentPartnerResolver = new PaymentPartnerResolver(DbContext);
        TransactionCommands = new TransactionCommands(DbContext, ExecutionContext, PaymentPartnerResolver);
        TransactionQueries = new TransactionQueries(DbContext, ExecutionContext);
        PaymentPartnerCommands = new PaymentPartnerCommands(DbContext, ExecutionContext);
        PaymentPartnerQueries = new PaymentPartnerQueries(DbContext, ExecutionContext);
        ReportQueries = new ReportQueries(DbContext, ExecutionContext);
    }

    protected static IExecutionContextAccessor MockExecutionContext(int personId)
    {
        var mock = new Mock<IExecutionContextAccessor>();
        mock.SetupGet(x => x.PersonId).Returns(personId);
        return mock.Object;
    }

    private static async Task<IExecutionContextAccessor> BuildExecutionContextMock(UnitTestingApiFactory factory)
    {
        var person = await new PeopleDataSeeder(factory).SeedPersonAsync();
        var mock = new Mock<IExecutionContextAccessor>();
        mock.SetupGet(x => x.PersonId).Returns(person.PersonId);
        mock.SetupGet(x => x.Email).Returns(new UserEmail(person.Email));
        mock.SetupGet(x => x.KeycloakUserId).Returns(Guid.TryParse(person.UserId, out var g) ? g : Guid.NewGuid());
        return mock.Object;
    }
}
