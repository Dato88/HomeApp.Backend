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
        public async Task<Budget> GetBudgetAsync()
        {
            Budget? budget = await _budgetFacade.GetBudgetAsync(1);

            return budget;
        }
    }
}
