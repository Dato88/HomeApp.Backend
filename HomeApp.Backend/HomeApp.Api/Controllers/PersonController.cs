using HomeApp.Library.Facades.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PersonController(IPersonFacade personFacade) : ControllerBase
{
    private readonly IPersonFacade _personFacade = personFacade;

    [HttpGet("person")]
    public async Task<IActionResult> GetPerson(CancellationToken cancellationToken)
    {
        var person = await _personFacade.GetUserPersonAsync(cancellationToken);

        return Ok(person);
    }
}
