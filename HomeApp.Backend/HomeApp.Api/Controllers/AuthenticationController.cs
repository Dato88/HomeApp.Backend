using HomeApp.Identity.Cruds.Interfaces;
using HomeApp.Identity.Models;
using HomeApp.Identity.Models.Register;
using HomeApp.Library.Email;
using HomeApp.Library.Models.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace HomeApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthenticationController(IUserCrud userCrud, UserManager<User> userManager, IEmailSender emailSender)
    : ControllerBase
{
    [HttpGet(Name = "GetAllUsers")]
    public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken cancellationToken)
    {
        var allUsers = await userCrud.GetAllUsersAsync(cancellationToken);

        return allUsers;
    }

    [HttpPost(Name = "Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDto registerUserDto,
        CancellationToken cancellationToken)
    {
        if (registerUserDto is null || !ModelState.IsValid)
            return BadRequest();

        IdentityResult result = await userCrud.RegisterAsync(registerUserDto, cancellationToken);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);

            return BadRequest(new RegistrationResponseDto { Errors = errors });
        }

        var token = await userManager.GenerateEmailConfirmationTokenAsync(registerUserDto);
        var param = new Dictionary<string, string?>
        {
            { "token", token },
            { "email", registerUserDto.Email }
        };
        var callback = QueryHelpers.AddQueryString(registerUserDto.ClientURI, param);
        var message = new Message(new string[] { registerUserDto.Email }, "Email Confirmation token", callback);

        await emailSender.SendEmailAsync(message, cancellationToken);

        return StatusCode(201);
    }
}