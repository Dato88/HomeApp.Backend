using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SharedKernel;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if ((isSuccess && error != Error.None) || (!isSuccess && error == Error.None))
            throw new ArgumentException("Invalid error state", nameof(error));

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    [JsonIgnore] public bool IsFailure => !IsSuccess;

    [JsonIgnore] public Error Error { get; }

    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Error? ErrorForSerialization => IsSuccess || Error == Error.None ? null : Error;

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error) =>
        _value = value;

    [JsonIgnore]
    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access Value of a failed result.");

    [JsonPropertyName("value")] public TValue? SerializableValue => IsSuccess ? _value : default;

    public static implicit operator Result<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Error.NullValue);

    public static Result<TValue> ValidationFailure(Error error) =>
        new(default, false, error);
}
