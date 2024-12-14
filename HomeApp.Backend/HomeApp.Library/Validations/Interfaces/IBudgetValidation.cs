namespace HomeApp.Library.Validations.Interfaces;

public interface IBudgetValidation
{
    /// <summary>
    /// Validate BudgetCell Table if UserId did Change in selected BudgetCell.
    /// <para>
    /// <code>Throws an exception if the userId is negative or Zero.</code>
    /// <code>Throws an exception if the budgetCellId is negative or Zero.</code>
    /// <code>Throws an exception if the selected userId is defferent from existing.</code>
    /// </para>
    /// </summary>
    /// <param name="userId">Selected UserId</param>
    /// <param name="budgetCellId">Selected BudgetCellId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when userId is negative or Zero.
    /// </exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when budgetCellId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when userId and budgetCellId not found in BudgetCell Table.
    /// </exception>
    Task ValidateBudgetCellForUserIdChangeAsync(int userId, int budgetCellId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetColumn Table if budgetColumnId exists.
    /// <para>
    /// <code>Throws an exception if the budgetColumnId is found in BudgetColumn Table.</code>
    /// </para>
    /// </summary>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when budgetColumnId is found in BudgetColumn Table.
    /// </exception>
    Task ValidateBudgetColumnIdExistsAsync(int budgetColumnId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetColumn Table if budgetColumnId does not exist.
    /// <para>
    /// <code>Throws an exception if the budgetColumnId is negative or Zero.</code>
    /// <code>Throws an exception if the budgetColumnId is not found in BudgetColumn Table.</code>
    /// </para>
    /// </summary>
    /// <param name="budgetColumnId">Selected BudgetColumnId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when userId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when budgetColumnId is not found in BudgetColumn Table.
    /// </exception>
    Task ValidateBudgetColumnIdExistsNotAsync(int budgetColumnId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetColumn Table if budgetColumnIndex and Name does already exist.
    /// <para>
    /// <code>Throws an exception if the index and name is found in BudgetColumn Table.</code>
    /// </para>
    /// </summary>
    /// <param name="index">Selected BudgetColumn Index</param>
    /// <param name="name">Selected BudgetColumn Name</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when index and name is found in BudgetColumn Table.
    /// </exception>
    Task ValidateBudgetColumnIndexAndNameExistsAsync(int index, string name, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetGroup Table if UserId did Change in selected BudgetGroup.
    /// <para>
    /// <code>Throws an exception if the userId is negative or Zero.</code>
    /// <code>Throws an exception if the budgetGroupId is negative or Zero.</code>
    /// <code>Throws an exception if the selected userId is defferent from existing.</code>
    /// </para>
    /// </summary>
    /// <param name="userId">Selected UserId</param>
    /// <param name="budgetGroupId">Selected BudgetGroupId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when userId is negative or Zero.
    /// </exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when budgetGroupId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when userId and budgetGroupId not found in BudgetGroup Table.
    /// </exception>
    Task ValidateBudgetGroupForUserIdChangeAsync(int userId, int budgetGroupId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetGroup Table if budgetGroupId does already exist.
    /// <para>
    /// <code>Throws an exception if the budgetGroupId is found in BudgetGroup Table.</code>
    /// </para>
    /// </summary>
    /// <param name="budgetGroupId">Selected BudgetGroupId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when budgetGroupId is found in BudgetColumn Table.
    /// </exception>
    Task ValidateBudgetGroupIdExistsAsync(int budgetGroupId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetGroup Table if budgetGroupId does not exist.
    /// <para>
    /// <code>Throws an exception if the budgetGroupId is negative or Zero.</code>
    /// <code>Throws an exception if the budgetGroupId is not found in BudgetGroup Table.</code>
    /// </para>
    /// </summary>
    /// <param name="budgetGroupId">Selected BudgetGroupId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when userId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when budgetGroupId is not found in BudgetGroup Table.
    /// </exception>
    Task ValidateBudgetGroupIdExistsNotAsync(int budgetGroupId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetGroup Table if budgetGroupIndex and Name does already exist.
    /// <para>
    /// <code>Throws an exception if the index and name is found in BudgetGroup Table.</code>
    /// </para>
    /// </summary>
    /// <param name="index">Selected BudgetGroup Index</param>
    /// <param name="name">Selected BudgetGroup Name</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when index and name is found in BudgetGroup Table.
    /// </exception>
    Task ValidateBudgetGroupIndexAndNameAlreadyExistsAsync(int index, string name,
        CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetRow Table if UserId did Change in selected BudgetRow.
    /// <para>
    /// <code>Throws an exception if the userId is negative or Zero.</code>
    /// <code>Throws an exception if the budgetRowId is negative or Zero.</code>
    /// <code>Throws an exception if the selected userId is defferent from existing.</code>
    /// </para>
    /// </summary>
    /// <param name="userId">Selected UserId</param>
    /// <param name="budgetRowId">Selected BudgetRowId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when userId is negative or Zero.
    /// </exception>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when budgetRowId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when userId and budgetRowId not found in BudgetRow Table.
    /// </exception>
    Task ValidateBudgetRowForUserIdChangeAsync(int userId, int budgetRowId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetRow Table if budgetRowId exists.
    /// <para>
    /// <code>Throws an exception if the budgetRowId is negative or Zero.</code>
    /// <code>Throws an exception if the budgetRowId is found in BudgetRow Table.</code>
    /// </para>
    /// </summary>
    /// <param name="budgetRowId">Selected BudgetRowId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when budgetRowId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when budgetRowId is found in BudgetRow Table.
    /// </exception>
    Task ValidateBudgetRowIdExistsAsync(int budgetRowId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate BudgetRow Table if budgetRowId does not exist.
    /// <para>
    /// <code>Throws an exception if the budgetRowId is negative or Zero.</code>
    /// <code>Throws an exception if the budgetRowId is not found in BudgetRow Table.</code>
    /// </para>
    /// </summary>
    /// <param name="budgetRowId">Selected BudgetRowId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when budgetRowId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when budgetRowId is not found in BudgetRow Table.
    /// </exception>
    Task ValidateBudgetRowIdExistsNotAsync(int budgetRowId, CancellationToken cancellationToken);

    /// <summary>
    /// Validate if the name is not an empty string.
    /// <para>
    /// <code>Throws an exception if the name is null or empty.</code>
    /// </para>
    /// </summary>
    /// <param name="name">The name to validate</param>
    /// <exception cref="System.ArgumentException">
    /// Thrown when name is null or empty.
    /// </exception>
    Task ValidateForEmptyStringAsync(string name);

    /// <summary>
    /// Validate if the index is positive.
    /// <para>
    /// <code>Throws an exception if the index is negative.</code>
    /// </para>
    /// </summary>
    /// <param name="index">The index to validate</param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when index is negative.
    /// </exception>
    Task ValidateForPositiveIndexAsync(int index);

    /// <summary>
    /// Validate if the userId exists in User Table.
    /// <para>
    /// <code>Throws an exception if the userId is negative or Zero.</code>
    /// <code>Throws an exception if the userId is not found in User Table.</code>
    /// </para>
    /// </summary>
    /// <param name="userId">Selected UserId</param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// Thrown when userId is negative or Zero.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown when userId is not found in User Table.
    /// </exception>
    Task ValidateForUserIdAsync(int userId, CancellationToken cancellationToken);
}
