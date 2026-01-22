using SixLabors.Fonts;
using TagsCloudContainer.Core.FontStrategies;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public static class FontFamilyResolver
{
    private static readonly IFontChoiceStrategy[] Strategies =
    [
        new ArialFontStrategy(),
        new HelveticaFontStrategy(),
        new MenloFontStrategy()
    ];
    
    private static readonly Result<IReadOnlyDictionary<string, IFontChoiceStrategy>> Map = CreateMap(Strategies);

    public static IReadOnlyCollection<string> Choices =>
        Strategies.Select(s => s.Key).ToArray();

    public static Result<FontFamily> Resolve(string choice)
    {
        var key = Normalize(choice);
        
        return Map.Bind(map =>
        {
            if (!map.TryGetValue(key, out var strategy))
                return Result<FontFamily>.Failure(
                    $"Font '{choice}' is not supported. Available options: {string.Join(", ", Choices)}");

            return strategy.Resolve();
        });
    }

    private static Result<IReadOnlyDictionary<string, IFontChoiceStrategy>> CreateMap(IEnumerable<IFontChoiceStrategy> src)
    {
        var fontChoiceStrategies = src as IFontChoiceStrategy[] ?? src.ToArray();

        if (fontChoiceStrategies.Length == 0)
            return Result<IReadOnlyDictionary<string, IFontChoiceStrategy>>.Failure(
                "The font source collection is empty.");

        var dict = new Dictionary<string, IFontChoiceStrategy>(StringComparer.OrdinalIgnoreCase);

        foreach (var s in fontChoiceStrategies)
            dict.Add(s.Key, s);

        dict.Add("", fontChoiceStrategies.First());

        return Result<IReadOnlyDictionary<string, IFontChoiceStrategy>>.Success(dict);
    }

    private static string Normalize(string? value) =>
        string.Concat(value).Trim().ToLowerInvariant();
}
