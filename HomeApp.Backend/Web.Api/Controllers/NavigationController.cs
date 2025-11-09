using Application.Models;

namespace Web.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class NavigationController : ControllerBase
{
    [HttpGet("navbar")]
    public IActionResult GetNavbar()
    {
        var navbarItems = new List<NavbarListItem>
        {
            new() { Name = "Dashboard", Link = "/dashboard", Icon = "bi bi-bank" },
            new() { Name = "Todo", Link = "/todo", Icon = "bi bi-list-task" },
            new() { Name = "Budget", Link = "/budget", Icon = "bi bi-bar-chart-line-fill" },
            new()
            {
                Name = "Settings",
                Link = "/settings",
                Icon = "bi bi-gear-fill",
                Sublist = new() { new() { Name = "Settings", Link = "/settings" }, }
            }
        };

        return Ok(navbarItems);
    }
}
