using Application.Features.Finance.Dtos.Eva;
using Application.Features.Finance.Reports.Queries;
using SharedKernel;

namespace Web.Api.Controllers;

[ApiController]
[Authorize(Roles = "ViewFinance")]
[Route("[controller]")]
public class ReportController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // E+A (Einnahmen/Ausgaben) report over one or more households, derived purely from transactions
    [HttpGet("eva")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<EvaReportResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetEvaAsync([FromQuery] int[] householdIds, [FromQuery] int year,
        CancellationToken cancellationToken)
    {
        var effectiveYear = year <= 0 ? DateTime.Now.Year : year;

        var response = await _mediator.Send(new GetEvaReportQuery(householdIds, effectiveYear));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
