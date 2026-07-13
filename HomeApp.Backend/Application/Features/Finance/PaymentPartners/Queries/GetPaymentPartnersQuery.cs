using Application.Features.Finance.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Finance.PaymentPartners.Queries;

public sealed record GetPaymentPartnersQuery : IRequest<Result<List<PaymentPartnerDto>>>;
