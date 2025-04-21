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
            new() { Name = "Dashboard", Link = "/dashboard", Icon = "dashboard" },
            new() { Name = "Todo", Link = "/todo", Icon = "task" },
            new() { Name = "Budget", Link = "/budget", Icon = "analytics" },
            new() { Name = "Settings", Link = "/settings", Icon = "settings" }
        };

        return Ok(navbarItems);
    }
}
