using HomeApp.DataAccess.Models;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers
{
    [ApiController]
    [Authorize(Policy = "ViewBudgetPolicy")]
    [Route("[controller]/[action]")]
    public class BudgetController(IBudgetFacade budgetFacade) : ControllerBase
    {
        [HttpGet(Name = "GetAll")]
        public async Task<Budget?> GetAllAsync(CancellationToken cancellationToken)
        {
            Budget? budget = await budgetFacade.GetBudgetAsync(1, cancellationToken);

            return budget;
        }

        [HttpPost(Name = "PostBudgetCell")]
        public async Task<BudgetCell> PostBudgetCellAsync([FromBody] BudgetCell budgetCell,
            CancellationToken cancellationToken)
        {
            await budgetFacade.CreateBudgetCellAsync(budgetCell, cancellationToken);

            return budgetCell;
        }

        [HttpPost(Name = "PostBudgetColumn")]
        public async Task<BudgetColumn> PostBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
            CancellationToken cancellationToken)
        {
            await budgetFacade.CreateBudgetColumnAsync(budgetColumn, cancellationToken);

            return budgetColumn;
        }

        [HttpPost(Name = "PostBudgetGroup")]
        public async Task<BudgetGroup> PostBudgetGroupAsync([FromBody] BudgetGroup budgetGroup,
            CancellationToken cancellationToken)
        {
            await budgetFacade.CreateBudgetGroupAsync(budgetGroup, cancellationToken);

            return budgetGroup;
        }

        [HttpPost(Name = "PostBudgetRow")]
        public async Task<BudgetRow> PostBudgetRowAsync([FromBody] BudgetRow budgetRow,
            CancellationToken cancellationToken)
        {
            await budgetFacade.CreateBudgetRowAsync(budgetRow, cancellationToken);

            return budgetRow;
        }

        [HttpPost(Name = "PutBudgetCell")]
        public async Task PutBudgetCellAsync([FromBody] BudgetCell budgetCell, CancellationToken cancellationToken)
        {
            await budgetFacade.UpdateBudgetCellAsync(budgetCell, cancellationToken);
        }

        [HttpPost(Name = "PutBudgetColumn")]
        public async Task PutBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
            CancellationToken cancellationToken)
        {
            await budgetFacade.UpdateBudgetColumnAsync(budgetColumn, cancellationToken);
        }

        [HttpPost(Name = "PutBudgetGroup")]
        public async Task PutBudgetGroupAsync([FromBody] BudgetGroup budgetGroup, CancellationToken cancellationToken)
        {
            await budgetFacade.UpdateBudgetGroupAsync(budgetGroup, cancellationToken);
        }

        [HttpPost(Name = "PutBudgetRow")]
        public async Task PutBudgetRowAsync([FromBody] BudgetRow budgetRow, CancellationToken cancellationToken)
        {
            await budgetFacade.UpdateBudgetRowAsync(budgetRow, cancellationToken);
        }

        [HttpPost(Name = "DeleteBudgetCell")]
        public async Task DeleteBudgetCellAsync(int id, CancellationToken cancellationToken)
        {
            await budgetFacade.DeleteBudgetCellAsync(id, cancellationToken);
        }

        [HttpPost(Name = "DeleteBudgetColumn")]
        public async Task DeleteBudgetColumnAsync(int id, CancellationToken cancellationToken)
        {
            await budgetFacade.DeleteBudgetColumnAsync(id, cancellationToken);
        }

        [HttpPost(Name = "DeleteBudgetGroup")]
        public async Task DeleteBudgetGroupAsync(int id, CancellationToken cancellationToken)
        {
            await budgetFacade.DeleteBudgetGroupAsync(id, cancellationToken);
        }

        [HttpPost(Name = "DeleteBudgetRow")]
        public async Task DeleteBudgetRowAsync(int id, CancellationToken cancellationToken)
        {
            await budgetFacade.DeleteBudgetRowAsync(id, cancellationToken);
        }
    }
}