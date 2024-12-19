using HomeApp.DataAccess.Models;
using HomeApp.Library.Cruds;
using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers;

[ApiController]
[Authorize]
// [Authorize(Policy = "ViewBudgetPolicy")]
[Route("[controller]/[action]")]
public class BudgetController(IBudgetFacade budgetFacade, IPersonCrud personCrud) : ControllerBase
{
    private readonly IBudgetFacade _budgetFacade = budgetFacade;
    private readonly IPersonCrud _personCrud = personCrud;

    [HttpGet(Name = "GetAll")]
    public async Task<Budget?> GetAllAsync(CancellationToken cancellationToken)
    {
        var budget = await _budgetFacade.GetBudgetAsync(cancellationToken);

        return budget;
    }

    [HttpPost(Name = "PostBudgetCell")]
    public async Task<BudgetCell> PostBudgetCellAsync([FromBody] BudgetCell budgetCell,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetCellAsync(budgetCell, cancellationToken);

        return budgetCell;
    }

    [HttpPost(Name = "PostBudgetColumn")]
    public async Task<BudgetColumn> PostBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetColumnAsync(budgetColumn, cancellationToken);

        return budgetColumn;
    }

    [HttpPost(Name = "PostBudgetGroup")]
    public async Task<BudgetGroup> PostBudgetGroupAsync([FromBody] BudgetGroup budgetGroup,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetGroupAsync(budgetGroup, cancellationToken);

        return budgetGroup;
    }

    [HttpPost(Name = "PostBudgetRow")]
    public async Task<BudgetRow> PostBudgetRowAsync([FromBody] BudgetRow budgetRow,
        CancellationToken cancellationToken)
    {
        await _budgetFacade.CreateBudgetRowAsync(budgetRow, cancellationToken);

        return budgetRow;
    }

    [HttpPost(Name = "PutBudgetCell")]
    public async Task PutBudgetCellAsync([FromBody] BudgetCell budgetCell, CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetCellAsync(budgetCell, cancellationToken);

    [HttpPost(Name = "PutBudgetColumn")]
    public async Task PutBudgetColumnAsync([FromBody] BudgetColumn budgetColumn,
        CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetColumnAsync(budgetColumn, cancellationToken);

    [HttpPost(Name = "PutBudgetGroup")]
    public async Task PutBudgetGroupAsync([FromBody] BudgetGroup budgetGroup, CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetGroupAsync(budgetGroup, cancellationToken);

    [HttpPost(Name = "PutBudgetRow")]
    public async Task PutBudgetRowAsync([FromBody] BudgetRow budgetRow, CancellationToken cancellationToken) =>
        await _budgetFacade.UpdateBudgetRowAsync(budgetRow, cancellationToken);

    [HttpPost(Name = "DeleteBudgetCell")]
    public async Task DeleteBudgetCellAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetCellAsync(id, cancellationToken);

    [HttpPost(Name = "DeleteBudgetColumn")]
    public async Task DeleteBudgetColumnAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetColumnAsync(id, cancellationToken);

    [HttpPost(Name = "DeleteBudgetGroup")]
    public async Task DeleteBudgetGroupAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetGroupAsync(id, cancellationToken);

    [HttpPost(Name = "DeleteBudgetRow")]
    public async Task DeleteBudgetRowAsync(int id, CancellationToken cancellationToken) =>
        await _budgetFacade.DeleteBudgetRowAsync(id, cancellationToken);
}
