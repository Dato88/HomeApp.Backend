using Application.Abstractions.Messaging;
using Domain.Entities.User;

namespace Application.Features.Users.Queries.GetAllUser;

public sealed record GetAllUserQuery : IQuery<IEnumerable<User>>;
