using HomeApp.Library.Models.BaseModels;
using HomeApp.Library.People.Dtos;

namespace HomeApp.Library.People.Queries;

public class GetUserPersonQuery : IRequest<BaseResponse<PersonDto>>
{
}
