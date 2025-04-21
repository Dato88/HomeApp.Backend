using Application.Abstractions.Messaging;

namespace Application.Users.Queries.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;
