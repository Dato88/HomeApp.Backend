using Application.Features.Todos.Dtos;
using MediatR;
using SharedKernel;

namespace Application.Features.Todos.Queries;

public record GetTodoByIdQuery(int Id) : IRequest<BaseResponse<GetToDoDto>>;
