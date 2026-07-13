using Application.Features.Finance.Dtos;
using Application.Features.Finance.PaymentPartners.Commands;
using Application.Features.Finance.PaymentPartners.Queries;
using SharedKernel;
using Web.Api.Requests.Finance;

namespace Web.Api.Controllers;

[ApiController]
[Authorize(Roles = "ViewFinance")]
[Route("[controller]")]
public class PaymentPartnerController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<PaymentPartnerDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetPaymentPartnersAsync(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetPaymentPartnersQuery());

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> RenamePaymentPartnerAsync(
        [FromBody] RenamePaymentPartnerRequest renamePaymentPartnerRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((RenamePaymentPartnerCommand)renamePaymentPartnerRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("merge")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> MergePaymentPartnersAsync(
        [FromBody] MergePaymentPartnersRequest mergePaymentPartnersRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((MergePaymentPartnersCommand)mergePaymentPartnersRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
