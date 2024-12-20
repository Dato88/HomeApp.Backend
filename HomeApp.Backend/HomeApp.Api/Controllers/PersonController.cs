using HomeApp.Library.Facades.Interfaces;
using HomeApp.Library.Models.Data_Transfer_Objects.PersonDtos;
using Microsoft.AspNetCore.Authorization;

namespace HomeApp.Api.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PersonController(IPersonFacade personFacade) : ControllerBase
{
    private readonly IPersonFacade _personFacade = personFacade;

    [HttpGet("person")]
    public async Task<PersonDto> GetPerson(CancellationToken cancellationToken)
    {
        var person = await _personFacade.GetUserPersonAsync(cancellationToken);

        return person;
    }
}
