using HomeApp.DataAccess.Models;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models;
using Microsoft.AspNetCore.Mvc;

namespace HomeApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BudgetController(IBudgetFacade budgetFacade) : ControllerBase
    {
        private readonly IBudgetFacade _budgetFacade = budgetFacade;

        [HttpGet(Name = "GetBudget")]
        public async Task<Budget> GetAsync()
        {
            Budget? budget = await _budgetFacade.GetBudgetAsync(1);

            return budget;
        }

        [HttpPost(Name = "PostCell")]
        public async Task<BudgetCell> PostBudgetCellAsync(BudgetCell budgetCell)
        {
            await _budgetFacade.PostBudgetCellAsync(budgetCell);

            return budgetCell;
        }

        [HttpPost(Name = "PostColumn")]
        public async Task<BudgetColumn> PostBudgetColumnAsync(BudgetColumn budgetColumn)
        {
            await _budgetFacade.PostBudgetColumnAsync(budgetColumn);

            return budgetColumn;
        }
        
        [HttpPost(Name = "PostGroup")]
        public async Task<BudgetGroup> PostBudgetGroupAsync(BudgetGroup budgetGroup)
        {
            await _budgetFacade.PostBudgetGroupAsync(budgetGroup);

            return budgetGroup;
        }


        [HttpPost(Name = "PostRow")]
        public async Task<BudgetRow> PostBudgetRowAsync(BudgetRow budgetRow)
        {
            await _budgetFacade.PostBudgetRowAsync(budgetRow);

            return budgetRow;
        }
    }
}
