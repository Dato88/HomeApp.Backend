using Application.Abstractions.Messaging;
using Domain.ValueObjects;

namespace Application.Features.Users.Queries.GetByEmail;

public sealed record GetUserByEmailQuery(UserEmail Email) : IQuery<UserResponse>;
