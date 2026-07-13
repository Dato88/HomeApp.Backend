using Domain.Entities.Finance;

namespace Application.Abstractions.FinanceModule;

public sealed record PaymentPartnerWithStats(PaymentPartner Partner, int TransactionCount, DateOnly? LastBookingDate);
