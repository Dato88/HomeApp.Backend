using Application.People.Dtos;
using MediatR;
using SharedKernel;

namespace Application.People.Queries;

public class GetUserPersonQuery : IRequest<BaseResponse<PersonDto>>
{
}
