using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console.Parsing;

public class FlagReader(IReadOnlyDictionary<string, string?> flags)
{
    public Result<string> RequirePath(string key, string name)
    {
        var rawResult = RequireString(key);
        if (!rawResult.IsSuccess)
            return Result<string>.Failure(rawResult.Error ?? Result<string>.UnknownError);

        var full = Path.GetFullPath(rawResult.Value!);

        var existsResult = Ensure.FileExists(full, $"{name} не найден: {full}");
        if (!existsResult.IsSuccess)
            return Result<string>.Failure(existsResult.Error ?? Result<string>.UnknownError);

        return Result<string>.Success(full);
    }

    private Result<string> RequireString(string key) =>
        flags.TryGetValue(key, out var v)
            ? Ensure.TrimmedNonEmpty(v, $"Обязательный параметр не задан: {key}")
            : Result<string>.Failure($"Обязательный параметр не задан: {key}");

    public Result<string> GetString(string key, string def) =>
        flags.TryGetValue(key, out var v)
            ? Ensure.TrimmedOrDefault(v, def)
            : Result<string>.Success(def);

    public Result<int> GetInt(string key, int def, IIntRule rule) =>
        flags.TryGetValue(key, out var v)
            ? Ensure.ParseIntOrDefault(v, def, rule)
            : Result<int>.Success(def);

    public Result<float> GetFloat(string key, float def, IFloatRule rule) =>
        flags.TryGetValue(key, out var v)
            ? Ensure.ParseFloatOrDefault(v, def, rule)
            : Result<float>.Success(def);

    public Result<Color> GetColor(string key, Color def) =>
        flags.TryGetValue(key, out var v)
            ? Ensure.ParseColorOrDefault(key, v, def)
            : Result<Color>.Success(def);

    public Result<bool> GetBool(string key, bool def) =>
        flags.TryGetValue(key, out var v)
            ? Ensure.ParseBoolOrDefault(v, def, key)
            : Result<bool>.Success(def);
}
