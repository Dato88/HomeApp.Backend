using Application.Abstractions.Messaging;
using Domain.Entities.User;

namespace Application.Users.Queries.GetAllUser;

public sealed record GetAllUserQuery : IQuery<IEnumerable<User>>;
