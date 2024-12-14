namespace HomeApp.Library.Tests.Crud.BudgetGroupCrudTests;

public class BaseBudgetGroupTest : BaseTest
{
    protected readonly BudgetGroupCrud _budgetGroupCrud;

    protected readonly Mock<IBudgetValidation> _budgetValidationMock;

    public BaseBudgetGroupTest()
    {
        _budgetValidationMock = new();
        _budgetValidationMock.DefaultValue = DefaultValue.Mock;
        _budgetValidationMock.SetupAllProperties();

        _budgetGroupCrud = new(_context, _budgetValidationMock.Object);
    }
}
