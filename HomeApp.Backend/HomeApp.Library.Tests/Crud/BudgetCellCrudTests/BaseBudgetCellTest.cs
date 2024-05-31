namespace HomeApp.Library.Tests.Crud.BudgetCellCrudTests
{
    public class BaseBudgetCellTest : BaseTest
    {
        protected readonly BudgetCellCrud _budgetCellCrud;

        protected readonly Mock<IBudgetValidation> _budgetValidationMock;

        public BaseBudgetCellTest()
        {
            _budgetValidationMock = new();
            _budgetValidationMock.DefaultValue = DefaultValue.Mock;
            _budgetValidationMock.SetupAllProperties();

            _budgetCellCrud = new(_context, _budgetValidationMock.Object);
        }
    }
}
