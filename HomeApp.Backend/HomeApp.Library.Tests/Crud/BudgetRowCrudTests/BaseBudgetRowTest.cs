namespace HomeApp.Library.Tests.Crud.BudgetRowCrudTests
{
    public class BaseBudgetRowTest : BaseTest
    {
        protected readonly BudgetRowCrud _budgetRowCrud;

        protected readonly Mock<IBudgetValidation> _budgetValidationMock;

        public BaseBudgetRowTest()
        {
            _budgetValidationMock = new();
            _budgetValidationMock.DefaultValue = DefaultValue.Mock;
            _budgetValidationMock.SetupAllProperties();

            _budgetRowCrud = new(_context, _budgetValidationMock.Object);
        }
    }
}