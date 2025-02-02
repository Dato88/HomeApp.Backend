using HomeApp.DataAccess.Models.Data_Transfer_Objects.TodoDtos;
using MediatR;

namespace HomeApp.Library.Todos.Commands;

public class GetUserTodosQuery : IRequest<IEnumerable<GetToDoDto>>
{
}
