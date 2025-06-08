using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SharedKernel;

public class Result
{
    protected Result(bool isSuccess, List<Error>? errors = null)
    {
        if ((isSuccess && errors is { Count: > 0 }) ||
            (!isSuccess && (errors == null || errors.Count == 0)))
            throw new ArgumentException("Invalid error state", nameof(errors));

        IsSuccess = isSuccess;
        Errors = errors ?? new List<Error>();
    }

    public bool IsSuccess { get; }

    [JsonIgnore] public bool IsFailure => !IsSuccess;

    [JsonIgnore] public List<Error> Errors { get; }

    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<Error>? SerializableErrors => IsSuccess ? null : Errors;

    public static Result Success() => new(true);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true);

    public static Result Failure(params Error[] errors) => new(false, errors.ToList());

    public static Result<TValue> Failure<TValue>(params Error[] errors) =>
        new(default, false, errors.ToList());
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, List<Error>? errors = null)
        : base(isSuccess, errors) =>
        _value = value;

    [JsonIgnore]
    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of failed result.");

    [JsonPropertyName("value")] public TValue? SerializableValue => IsSuccess ? _value : default;

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static new Result<TValue> Failure(params Error[] errors) =>
        new(default, false, errors.ToList());
}
