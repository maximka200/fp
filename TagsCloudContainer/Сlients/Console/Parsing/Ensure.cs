using System.Globalization;
using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.ColorParsing;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal static class Ensure
{
    public static Result<Unit> True(bool condition, string message) =>
        condition
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(message);

    public static Result<Unit> NextTokenIsValue(string next, string key) =>
        !next.StartsWith("--", StringComparison.Ordinal)
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure($"Ожидалось значение после {key}");

    public static Result<Unit> FileExists(string path, string message) =>
        File.Exists(path)
            ? Result<Unit>.Success(Unit.Value)
            : Result<Unit>.Failure(message);

    public static Result<string> TrimmedNonEmpty(string? value, string message)
    {
        var s = string.Concat(value).Trim();
        return string.IsNullOrWhiteSpace(s)
            ? Result<string>.Failure(message)
            : Result<string>.Success(s);
    }

    public static Result<string> TrimmedOrDefault(string? value, string def)
    {
        var s = string.Concat(value).Trim();
        return !string.IsNullOrWhiteSpace(s)
            ? Result<string>.Success(s)
            : Result<string>.Success(def);
    }

    public static Result<int> ParseIntOrDefault(string? value, int def, IIntRule rule) =>
        ParseOrDefault(
            value,
            def,
            parse: s => int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture),
            validate: rule.Validate,
            errorMessage: () => $"Некорректный {rule.Label}: {value}"
        );

    public static Result<float> ParseFloatOrDefault(string? value, float def, IFloatRule rule) =>
        ParseOrDefault(
            value,
            def,
            parse: s => float.Parse(s, NumberStyles.Float, CultureInfo.InvariantCulture),
            validate: rule.Validate,
            errorMessage: () => $"Некорректный {rule.Label}: {value}"
        );

    public static Result<Color> ParseColorOrDefault(string key, string? value, Color def) =>
        ParseOrDefault(
            value,
            def,
            parse: s => ColorParser.Parse(key, s),
            validate: c => Result<Color>.Success(c),
            errorMessage: () =>
                $"Некорректный {key}: {value}. Пример: {key} #ffffff"
        );

    public static Result<bool> ParseBoolOrDefault(string? value, bool def, string label) =>
        ParseOrDefault(
            value,
            def,
            parse: s => BoolParser.Parse(s, label),
            validate: b => Result<bool>.Success(b),
            errorMessage: () => $"Некорректный {label}: {value}"
        );

    private static Result<T> ParseOrDefault<T>(
        string? value,
        T def,
        Func<string, T> parse,
        Func<T, Result<T>> validate,
        Func<string>? errorMessage = null)
    {
        var s = string.Concat(value).Trim();

        if (string.IsNullOrWhiteSpace(s))
            return Result<T>.Success(def);

        // здесь специально оставил исключения в кастомных парсерах (Bool, Color),
        // чтобы не было отличий от поведения встроенных парсеров
        try
        {
            var parsed = parse(s);
            return validate(parsed);
        }
        catch (Exception)
        {
            return Result<T>.Failure(
                errorMessage?.Invoke() ?? Result<T>.UnknownError);
        }
    }
}
