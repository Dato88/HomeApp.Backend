namespace HomeApp.Library.Tests.Crud.PersonCrudTests;

public class BasePersonTest : BaseTest
{
    protected readonly PersonCrud _personCrud;

    protected readonly Mock<IPersonValidation> _personValidationMock;

    public BasePersonTest(UnitTestingApiFactory unitTestingApiFactory) : base(unitTestingApiFactory)
    {
        _personValidationMock = new Mock<IPersonValidation>();
        _personValidationMock.DefaultValue = DefaultValue.Mock;
        _personValidationMock.SetupAllProperties();

        _personCrud = new PersonCrud(DbContext, _personValidationMock.Object);
    }
}
