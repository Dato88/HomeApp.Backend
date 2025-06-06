using Application.Features.People.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.People.Queries;

public sealed record GetUserPersonQuery : IRequest<Result<PersonResponse>>
{
}
