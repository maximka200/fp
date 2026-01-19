using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class FlagStore
{
    private static readonly IReadOnlyDictionary<string, string> Allowed =
        CliFlags.All.ToDictionary(x => x, x => x, StringComparer.OrdinalIgnoreCase);

    public static Result<bool> Put(
        IDictionary<string, string?> flags,
        string key,
        string? value)
    {
        if (!Allowed.ContainsKey(key))
            return Result<bool>.Failure($"Неизвестный флаг: {key}");
        
        var trimmed = string.Concat(value).Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            return Result<bool>.Failure($"Пустое значение для {key}");
        
        return !flags.TryAdd(key, trimmed) ? Result<bool>.Failure($"Флаг указан дважды: {key}")
            : Result<bool>.Success(true);
    }
}