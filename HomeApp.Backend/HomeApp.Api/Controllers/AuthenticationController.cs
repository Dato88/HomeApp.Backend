using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Models;
using HomeApp.Identity.Models.Register;
using Microsoft.AspNetCore.Identity;

namespace HomeApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthenticationController(IUserCrud userCrud, UserManager<User> userManager) : ControllerBase
{
    private readonly IUserCrud _userCrud = userCrud;

    [HttpPost(Name = "Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto,
        CancellationToken cancellationToken)
    {
        if (registerUserDto == null || !ModelState.IsValid)
            return BadRequest();

        IdentityResult result = await _userCrud.RegisterAsync(registerUserDto, cancellationToken);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            return BadRequest(new RegistrationResponseDto { Errors = errors });
        }

        return StatusCode(201);
    }
}