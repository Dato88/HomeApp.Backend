using SharedKernel;

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

    public static Error NotFoundById(int id) => Error.NotFound(
        "People.NotFoundById",
        $"The person with the Id = '{id}' was not found");
}
