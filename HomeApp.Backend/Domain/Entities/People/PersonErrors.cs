using SharedKernel;
using SharedKernel.ValueObjects;

namespace Domain.Entities.People;

public static class PersonErrors
{
    public static readonly Error NotFoundByEmail = Error.NotFound(
        "People.NotFoundByEmail",
        "The person with the specified email was not found");

    public static readonly Error NotFoundAll = Error.NotFound(
        "People.NotFoundAll",
        "There are no people in the database");

    public static readonly Error NotFound = Error.NotFound(
        "People.NotFound",
        "There is no person in the database");

    public static Error NotFoundById(PersonId personId) => Error.NotFound(
        "People.NotFoundById",
        $"The person with the Id = '{personId}' was not found");

    public static Error CreateFailedWithMessage(string message) => Error.Failure(
        "Person.CreateFailedWithMessage",
        $"The person could not be created with message = '{message}'");

    public static Error DeleteFailed(PersonId personId) => Error.Failure(
        "Person.DeleteFailed",
        $"The person with the id = '{personId}' could not be deleted");

    public static Error UpdateFailedWithMessage(string message) => Error.Failure(
        "Person.UpdateFailedWithMessage",
        $"The person could not be updated with message = '{message}'");
}
