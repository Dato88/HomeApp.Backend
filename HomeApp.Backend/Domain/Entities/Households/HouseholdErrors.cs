using SharedKernel;

namespace Domain.Entities.Households;

public static class HouseholdErrors
{
    public static readonly Error NotFoundAll = Error.NotFound(
        "Household.NotFoundAll",
        "There are no households in the database");

    public static Error NotFoundById(int householdId) => Error.NotFound(
        "Household.NotFoundById",
        $"The household with the Id = '{householdId}' was not found");

    public static Error CreateFailedWithMessage(string message) => Error.Failure(
        "Household.CreateFailedWithMessage",
        $"The household could not be created with message = '{message}'");

    public static Error UpdateFailedWithMessage(string message) => Error.Failure(
        "Household.UpdateFailedWithMessage",
        $"The household could not be updated with message = '{message}'");

    public static Error DeleteFailedWithMessage(string message) => Error.Failure(
        "Household.DeleteFailedWithMessage",
        $"The household could not be deleted with message = '{message}'");

    public static Error AddMemberFailedWithMessage(string message) => Error.Failure(
        "Household.AddMemberFailedWithMessage",
        $"The household member could not be added with message = '{message}'");

    public static Error RemoveMemberFailedWithMessage(string message) => Error.Failure(
        "Household.RemoveMemberFailedWithMessage",
        $"The household member could not be removed with message = '{message}'");
}
