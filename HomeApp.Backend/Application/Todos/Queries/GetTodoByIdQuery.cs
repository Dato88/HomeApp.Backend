using Application.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Todos.Queries;

public record GetTodoByIdQuery(int Id) : IRequest<BaseResponse<GetToDoDto>>;
