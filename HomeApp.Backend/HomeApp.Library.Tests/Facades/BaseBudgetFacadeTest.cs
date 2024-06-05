using HomeApp.Library.Cruds.Interfaces;
using HomeApp.Library.Cruds;
using HomeApp.Library.Facades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApp.Library.Tests.Facades
{
    public class BaseBudgetFacadeTest : BaseTest
    {
        protected readonly BudgetFacade _budgetFacade;

        protected readonly Mock<IBudgetCellCrud> _budgetCellCrudMock;
        protected readonly Mock<IBudgetColumnCrud> _budgetColumnCrudMock;
        protected readonly Mock<IBudgetGroupCrud> _budgetGroupCrudMock;
        protected readonly Mock<IBudgetRowCrud> _budgetRowCrudMock;

        public BaseBudgetFacadeTest()
        {
            _budgetCellCrudMock = new();
            _budgetCellCrudMock.DefaultValue = DefaultValue.Mock;
            _budgetCellCrudMock.SetupAllProperties();

            _budgetColumnCrudMock = new();
            _budgetColumnCrudMock.DefaultValue = DefaultValue.Mock;
            _budgetColumnCrudMock.SetupAllProperties();

            _budgetGroupCrudMock = new();
            _budgetGroupCrudMock.DefaultValue = DefaultValue.Mock;
            _budgetGroupCrudMock.SetupAllProperties();

            _budgetRowCrudMock = new();
            _budgetRowCrudMock.DefaultValue = DefaultValue.Mock;
            _budgetRowCrudMock.SetupAllProperties();

            _budgetFacade = new(_budgetCellCrudMock.Object, _budgetColumnCrudMock.Object, _budgetGroupCrudMock.Object, _budgetRowCrudMock.Object);
        }
    }
}
