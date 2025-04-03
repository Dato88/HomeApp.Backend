using Application.Features.People.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.People.Queries;

public class GetUserPersonQuery : IRequest<Result<PersonResponse>>
{
}
