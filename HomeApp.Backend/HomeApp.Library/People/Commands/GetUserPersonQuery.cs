using HomeApp.DataAccess.Models.Data_Transfer_Objects.PersonDtos;
using HomeApp.Library.Models.BaseModels;
using MediatR;

namespace HomeApp.Library.People.Commands;

public class GetUserPersonQuery : IRequest<BaseResponse<PersonDto>>
{
}
