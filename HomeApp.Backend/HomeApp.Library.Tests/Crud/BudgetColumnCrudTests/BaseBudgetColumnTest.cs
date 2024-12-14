namespace HomeApp.Library.Tests.Crud.BudgetColumnCrudTests;

public class BaseBudgetColumnTest : BaseTest
{
    protected readonly BudgetColumnCrud _budgetColumnCrud;

    protected readonly Mock<IBudgetValidation> _budgetValidationMock;

    public BaseBudgetColumnTest()
    {
        _budgetValidationMock = new();
        _budgetValidationMock.DefaultValue = DefaultValue.Mock;
        _budgetValidationMock.SetupAllProperties();

        _budgetColumnCrud = new(_context, _budgetValidationMock.Object);
    }
}
