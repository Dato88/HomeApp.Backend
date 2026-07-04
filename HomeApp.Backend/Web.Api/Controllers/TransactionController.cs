using Application.Features.Finance.Dtos;
using Application.Features.Finance.Transactions.Commands;
using Application.Features.Finance.Transactions.Queries;
using SharedKernel;
using Web.Api.Requests.Finance;

namespace Web.Api.Controllers;

[ApiController]
[Authorize(Roles = "ViewFinance")]
[Route("[controller]")]
public class TransactionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<TransactionListResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetTransactionsAsync([FromQuery] int accountId, [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to, [FromQuery] int? categoryId, [FromQuery] bool? uncategorized,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var response = await _mediator.Send(new GetTransactionsQuery(accountId, from, to, categoryId,
            uncategorized, page, pageSize));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostTransactionAsync(
        [FromBody] CreateTransactionRequest createTransactionRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateTransactionCommand)createTransactionRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UpdateTransactionAsync(
        [FromBody] UpdateTransactionRequest updateTransactionRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((UpdateTransactionCommand)updateTransactionRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteTransactionAsync([FromQuery] int transactionId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteTransactionCommand(transactionId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("category")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> SetTransactionCategoryAsync(
        [FromBody] SetTransactionCategoryRequest setTransactionCategoryRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((SetTransactionCategoryCommand)setTransactionCategoryRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
