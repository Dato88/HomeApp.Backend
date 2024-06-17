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

        [HttpGet(Name = "GetAll")]
        public async Task<Budget?> GetAllAsync()
        {
            Budget? budget = await _budgetFacade.GetBudgetAsync(1);

            return budget;
        }

        [HttpPost(Name = "PostBudgetCell")]
        public async Task<BudgetCell> PostBudgetCellAsync([FromBody] BudgetCell budgetCell)
        {
            await _budgetFacade.CreateBudgetCellAsync(budgetCell);

            return budgetCell;
        }

        [HttpPost(Name = "PostBudgetColumn")]
        public async Task<BudgetColumn> PostBudgetColumnAsync([FromBody] BudgetColumn budgetColumn)
        {
            await _budgetFacade.CreateBudgetColumnAsync(budgetColumn);

            return budgetColumn;
        }

        [HttpPost(Name = "PostBudgetGroup")]
        public async Task<BudgetGroup> PostBudgetGroupAsync([FromBody] BudgetGroup budgetGroup)
        {
            await _budgetFacade.CreateBudgetGroupAsync(budgetGroup);

            return budgetGroup;
        }

        [HttpPost(Name = "PostBudgetRow")]
        public async Task<BudgetRow> PostBudgetRowAsync([FromBody] BudgetRow budgetRow)
        {
            await _budgetFacade.CreateBudgetRowAsync(budgetRow);

            return budgetRow;
        }

        [HttpPost(Name = "PutBudgetCell")]
        public async Task PutBudgetCellAsync([FromBody] BudgetCell budgetCell)
        {
            await _budgetFacade.UpdateBudgetCellAsync(budgetCell);
        }

        [HttpPost(Name = "PutBudgetColumn")]
        public async Task PutBudgetColumnAsync([FromBody] BudgetColumn budgetColumn)
        {
            await _budgetFacade.UpdateBudgetColumnAsync(budgetColumn);
        }
        
        [HttpPost(Name = "PutBudgetGroup")]
        public async Task PutBudgetGroupAsync([FromBody] BudgetGroup budgetGroup)
        {
            await _budgetFacade.UpdateBudgetGroupAsync(budgetGroup);
        }
        
        [HttpPost(Name = "PutBudgetRow")]
        public async Task PutBudgetRowAsync([FromBody] BudgetRow budgetRow)
        {
            await _budgetFacade.UpdateBudgetRowAsync(budgetRow);
        }
    }
}
