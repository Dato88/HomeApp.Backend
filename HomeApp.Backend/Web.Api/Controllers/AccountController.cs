using Application.Features.Finance.Accounts.Commands;
using Application.Features.Finance.Accounts.Queries;
using Application.Features.Finance.Dtos;
using SharedKernel;
using Web.Api.Requests.Finance;

namespace Web.Api.Controllers;

[ApiController]
[Authorize(Roles = "ViewFinance")]
[Route("[controller]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<AccountDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetAccountsAsync(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAccountsQuery());

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostAccountAsync([FromBody] CreateAccountRequest createAccountRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateAccountCommand)createAccountRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UpdateAccountAsync([FromBody] UpdateAccountRequest updateAccountRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((UpdateAccountCommand)updateAccountRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteAccountAsync([FromQuery] int accountId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteAccountCommand(accountId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> ShareAccountAsync([FromBody] ShareAccountRequest shareAccountRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((ShareAccountCommand)shareAccountRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("share")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UnshareAccountAsync([FromQuery] int accountId, [FromQuery] int householdId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new UnshareAccountCommand(accountId, householdId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
