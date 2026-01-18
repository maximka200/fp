namespace TagsCloudContainer.Result;

public static class ResultExtensions
{
    public static Result<T> Bind<T>(
        this Result<T> result,
        Func<T, Result<T>> next)
    {
        if (!result.IsSuccess)
            return Result<T>.Failure(result.Error!);

        return next(result.Value!);
    }
}
