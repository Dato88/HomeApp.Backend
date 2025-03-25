using SharedKernel;

namespace Domain.Entities.User;

public static class UserErrors
{
    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "The user with the specified email was not found");

    public static readonly Error NotFoundAll = Error.NotFound(
        "Users.NotFoundAll",
        "There are no users in the database");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static readonly Error EmailNotConfirmed = Error.Conflict(
        "Users.EmailNotConfirmed",
        "The provided email is not confirmed");

    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static Error Invalid2Step(Guid userId) => Error.NotFound(
        "Users.Invalid2Step",
        $"The user with the Id = '{userId}' send invalid 2 step verification");

    public static Error LockedOut(Guid userId) => Error.NotFound(
        "Users.LockedOut",
        $"The user with the Id = '{userId}' is locked out");

    public static Error Unauthorized() => Error.Failure(
        "Users.Unauthorized",
        "You are not authorized to perform this action.");
}
