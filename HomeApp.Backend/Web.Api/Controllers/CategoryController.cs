using Application.Features.Finance.Categories.Commands;
using Application.Features.Finance.Categories.Queries;
using Application.Features.Finance.Dtos;
using SharedKernel;
using Web.Api.Requests.Finance;

namespace Web.Api.Controllers;

[ApiController]
[Authorize(Roles = "ViewFinance")]
[Route("[controller]")]
public class CategoryController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<List<CategoryDto>>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> GetCategoriesAsync([FromQuery] int householdId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetCategoriesQuery(householdId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> PostCategoryAsync([FromBody] CreateCategoryRequest createCategoryRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((CreateCategoryCommand)createCategoryRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpPatch("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> UpdateCategoryAsync([FromBody] UpdateCategoryRequest updateCategoryRequest,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send((UpdateCategoryCommand)updateCategoryRequest);

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }

    [HttpDelete("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result<int>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
    public async Task<IActionResult> DeleteCategoryAsync([FromQuery] int categoryId,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new DeleteCategoryCommand(categoryId));

        if (response.IsSuccess) return Ok(response);

        return BadRequest(response.Error);
    }
}
