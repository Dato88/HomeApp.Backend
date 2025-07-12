using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SharedKernel;

public class Result
{
    // Ein erfolgreicher Result darf keine Fehler enthalten,
    // ein Fehler-Result muss Fehler enthalten.
    protected Result(bool isSuccess, List<Error>? errors = null)
    {
        // Validierung: Erfolgreiches Ergebnis darf keine Fehler enthalten,
        // fehlgeschlagenes Ergebnis MUSS Fehler enthalten.
        if ((isSuccess && errors is { Count: > 0 }) ||
            (!isSuccess && (errors == null || errors.Count == 0)))
            throw new ArgumentException("Invalid error state", nameof(errors));

        IsSuccess = isSuccess;
        Errors = errors ?? new List<Error>();
    }

    public bool IsSuccess { get; }

    // Praktischer Helfer für if (result.IsFailure)-Abfragen.
    // Wird nicht serialisiert.
    [JsonIgnore] public bool IsFailure => !IsSuccess;

    // Enthält alle Fehlerobjekte. Wird auch nicht direkt serialisiert,
    // sondern über SerializableErrors.
    [JsonIgnore] public List<Error> Errors { get; }

    // Wird nur serialisiert, wenn es sich um ein Fehlerergebnis
    // Wenn IsSuccess = true, wird null zurückgegeben
    // und der Fehlerblock entfällt in JSON.
    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Error>? SerializableErrors => IsSuccess ? null : Errors;

    // Erstellt ein erfolgreiches Ergebnis ohne Wert
    // (z.B. für Result<void>-Szenarien).
    public static Result Success() => new(true);

    // Erstellt ein erfolgreiches generisches Ergebnis mit Wert (Result<T>)
    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true);

    // Erstellt ein Result mit Fehlern – z.B. bei Validierungsfehlern.
    public static Result Failure(params Error[] errors) => new(false, errors.ToList());

    // Erstellt ein generisches Result<T> mit Fehlern und leerem Wert.
    public static Result<TValue> Failure<TValue>(params Error[] errors) =>
        new(default, false, errors.ToList());
}

public class Result<TValue> : Result
{
    // Verwaltet zusätzlich einen generischen Wert,
    // falls der Vorgang erfolgreich war.
    private readonly TValue? _value;

    // Ruft Basiskonstruktor auf und speichert den Wert.
    // Fehler-Validierung geschieht über Basisklasse.
    public Result(TValue? value, bool isSuccess, List<Error>? errors = null)
        : base(isSuccess, errors) =>
        _value = value;

    // Zugriff auf den gespeicherten Wert – nur erlaubt,
    // wenn IsSuccess true ist, sonst Exception.
    // NotNull ist ein Hinweis für Codeanalyse.
    [JsonIgnore]
    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of failed result.");

    // JSON-Serialisierung: Gibt den Wert nur zurück,
    // wenn Erfolg, sonst null.
    [JsonPropertyName("value")] public TValue? SerializableValue => IsSuccess ? _value : default;

    // Erlaubt automatische Umwandlung von TValue in Result<TValue–
    // nützlich z.B. in Handlern.
    // Bei null wird ein Fehler mit dem speziellen Fehler
    // Error.NullValue zurückgegeben.
    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    // Diese Failure-Methode überschreibt die aus der Basisklasse für
    // generische Ergebnisse.
    public static new Result<TValue> Failure(params Error[] errors) =>
        new(default, false, errors.ToList());
}
