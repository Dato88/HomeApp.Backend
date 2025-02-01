using HomeApp.DataAccess.Cruds.People;
using HomeApp.DataAccess.Tests.Helper;
using HomeApp.DataAccess.Validations.Interfaces;
using Moq;

namespace HomeApp.DataAccess.Tests.Cruds.People;

public class BasePersonCommandsTest : BaseTest
{
    protected readonly PersonCommands PersonCommands;

    protected readonly Mock<IPersonValidation> PersonValidationMock;

    public BasePersonCommandsTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        PersonValidationMock = new Mock<IPersonValidation>();
        PersonValidationMock.DefaultValue = DefaultValue.Mock;
        PersonValidationMock.SetupAllProperties();

        PersonCommands = new PersonCommands(DbContext, PersonValidationMock.Object);
    }
}
