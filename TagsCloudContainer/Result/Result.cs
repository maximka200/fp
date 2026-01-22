namespace TagsCloudContainer.Result;

public class Result<T>
{
    public bool IsSuccess { get; }
    
    public T? Value { get; }
    public string? Error { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(string error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);

    public const string UnknownError = "Unknown error";

    public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> func)
    {
        return !IsSuccess ? Result<TResult>.Failure(Error ?? UnknownError) : func(Value!);
    }
    
    public Result<TResult> Map<TResult>(Func<T, TResult> func)
    {
        return !IsSuccess ? Result<TResult>.Failure(Error ?? UnknownError) : Result<TResult>.Success(func(Value!));
    }

    public Result<T> OnFailure(Action<string> action)
    {
        if (!IsSuccess && Error != null)
            action(Error);

        return this;
    }
    
    public T ValueOr(T defaultValue) => IsSuccess ? Value! : defaultValue;
}