using Application.Features.Finance.Dtos.Eva;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.Reports.Queries;

public sealed record GetEvaReportQuery(IReadOnlyList<int> HouseholdIds, int Year)
    : IRequest<Result<EvaReportResponse>>;
