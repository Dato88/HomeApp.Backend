using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Facades;
using HomeApp.Library.Facades.Interfaces;
using Microsoft.Extensions.Logging;

namespace HomeApp.Library.Tests.Facades;

public class BaseBudgetFacadeTest : BaseTest
{
    protected readonly Mock<IBudgetCellCrud> _budgetCellCrudMock;
    protected readonly Mock<IBudgetColumnCrud> _budgetColumnCrudMock;
    protected readonly BudgetFacade _budgetFacade;
    protected readonly Mock<IBudgetGroupCrud> _budgetGroupCrudMock;
    protected readonly Mock<IBudgetRowCrud> _budgetRowCrudMock;
    protected readonly Mock<ILogger<BudgetFacade>> _iLogger;
    protected readonly Mock<IPersonFacade> _personFacadeMock;

    public BaseBudgetFacadeTest()
    {
        _budgetCellCrudMock = new Mock<IBudgetCellCrud>();
        _budgetCellCrudMock.DefaultValue = DefaultValue.Mock;
        _budgetCellCrudMock.SetupAllProperties();

        _budgetColumnCrudMock = new Mock<IBudgetColumnCrud>();
        _budgetColumnCrudMock.DefaultValue = DefaultValue.Mock;
        _budgetColumnCrudMock.SetupAllProperties();

        _budgetGroupCrudMock = new Mock<IBudgetGroupCrud>();
        _budgetGroupCrudMock.DefaultValue = DefaultValue.Mock;
        _budgetGroupCrudMock.SetupAllProperties();

        _budgetRowCrudMock = new Mock<IBudgetRowCrud>();
        _budgetRowCrudMock.DefaultValue = DefaultValue.Mock;
        _budgetRowCrudMock.SetupAllProperties();

        _personFacadeMock = new Mock<IPersonFacade>();
        _personFacadeMock.DefaultValue = DefaultValue.Mock;
        _personFacadeMock.SetupAllProperties();

        _iLogger = new Mock<ILogger<BudgetFacade>>();
        _iLogger.DefaultValue = DefaultValue.Mock;
        _iLogger.SetupAllProperties();

        _budgetFacade = new BudgetFacade(_budgetCellCrudMock.Object, _budgetColumnCrudMock.Object,
            _budgetGroupCrudMock.Object,
            _budgetRowCrudMock.Object, _personFacadeMock.Object, _iLogger.Object);
    }
}
