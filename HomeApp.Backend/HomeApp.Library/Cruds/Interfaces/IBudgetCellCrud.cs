namespace HomeApp.Library.Cruds.Interfaces;

public interface IBudgetCellCrud
{
    /// <summary>
    ///     Erstellt eine neue BudgetCell.
    /// </summary>
    /// <param name="budgetCell">Die zu erstellende BudgetCell.</param>
    /// <returns>Die erstellte BudgetCell.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Wird geworfen, wenn <paramref name="budgetCell" /> null ist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn UserId, BudgetRowId, BudgetColumnId oder BudgetGroupId nicht existieren.
    /// </exception>
    Task<BudgetCell> CreateAsync(BudgetCell budgetCell, CancellationToken cancellationToken);

    /// <summary>
    ///     Löscht eine BudgetCell anhand ihrer ID.
    /// </summary>
    /// <param name="id">Die ID der zu löschenden BudgetCell.</param>
    /// <returns>True, wenn die Löschung erfolgreich war, andernfalls False.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn die BudgetCell mit der angegebenen ID nicht gefunden wird.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Findet eine BudgetCell anhand ihrer ID.
    /// </summary>
    /// <param name="id">Die ID der zu findenden BudgetCell.</param>
    /// <returns>Die gefundene BudgetCell.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn die BudgetCell mit der angegebenen ID nicht gefunden wird.
    /// </exception>
    Task<BudgetCell> FindByIdAsync(int id, CancellationToken cancellationToken, params string[] includes);

    /// <summary>
    ///     Gibt alle BudgetCells zurück.
    /// </summary>
    /// <returns>Eine Liste aller BudgetCells.</returns>
    Task<IEnumerable<BudgetCell>> GetAllAsync(CancellationToken cancellationToken, params string[] includes);

    /// <summary>
    ///     Gibt alle BudgetCells für einen bestimmten Benutzer zurück.
    /// </summary>
    /// <param name="userId">Die ID des Benutzers.</param>
    /// <returns>Eine Liste aller BudgetCells für den angegebenen Benutzer.</returns>
    Task<IEnumerable<BudgetCell>> GetAllAsync(int userId, CancellationToken cancellationToken,
        params string[] includes);

    /// <summary>
    ///     Aktualisiert eine bestehende BudgetCell.
    /// </summary>
    /// <param name="budgetCell">Die zu aktualisierende BudgetCell.</param>
    /// <exception cref="ArgumentNullException">
    ///     Wird geworfen, wenn <paramref name="budgetCell" /> null ist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn UserId, BudgetRowId, BudgetColumnId oder BudgetGroupId nicht existieren oder wenn
    ///     die BudgetCell mit der angegebenen ID nicht gefunden wird.
    /// </exception>
    Task UpdateAsync(BudgetCell budgetCell, CancellationToken cancellationToken);
}
