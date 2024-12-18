namespace HomeApp.Library.Cruds.Interfaces;

public interface IBudgetColumnCrud
{
    /// <summary>
    ///     Erstellt eine neue BudgetColumn.
    /// </summary>
    /// <param name="budgetColumn">Die zu erstellende BudgetColumn.</param>
    /// <returns>Die erstellte BudgetColumn.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Wird geworfen, wenn <paramref name="budgetColumn" /> null ist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn BudgetColumnId, Name oder Index nicht existieren oder wenn die BudgetColumn mit der angegebenen
    ///     ID nicht gefunden wird.
    /// </exception>
    Task<BudgetColumn> CreateAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken);

    /// <summary>
    ///     Löscht eine BudgetColumn anhand ihrer ID.
    /// </summary>
    /// <param name="id">Die ID der zu löschenden BudgetColumn.</param>
    /// <returns>True, wenn die Löschung erfolgreich war, andernfalls False.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn die BudgetColumn mit der angegebenen ID nicht gefunden wird.
    /// </exception>
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    ///     Findet eine BudgetColumn anhand ihrer ID.
    /// </summary>
    /// <param name="id">Die ID der zu findenden BudgetColumn.</param>
    /// <returns>Die gefundene BudgetColumn.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn die BudgetColumn mit der angegebenen ID nicht gefunden wird.
    /// </exception>
    Task<BudgetColumn> FindByIdAsync(int id, CancellationToken cancellationToken, params string[] includes);

    /// <summary>
    ///     Gibt alle BudgetColumns zurück.
    /// </summary>
    /// <returns>Eine Liste aller BudgetColumns.</returns>
    Task<IEnumerable<BudgetColumn>> GetAllAsync(CancellationToken cancellationToken, params string[] includes);

    /// <summary>
    ///     Aktualisiert eine bestehende BudgetColumn.
    /// </summary>
    /// <param name="budgetColumn">Die zu aktualisierende BudgetColumn.</param>
    /// <exception cref="ArgumentNullException">
    ///     Wird geworfen, wenn <paramref name="budgetColumn" /> null ist.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     Wird geworfen, wenn BudgetColumnId, Name oder Index nicht existieren oder wenn die BudgetColumn mit der angegebenen
    ///     ID nicht gefunden wird.
    /// </exception>
    Task UpdateAsync(BudgetColumn budgetColumn, CancellationToken cancellationToken);
}
