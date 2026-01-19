using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients;

public class ClientStrategySelector
{
    private readonly IReadOnlyDictionary<string, IClientStrategy> strategies;
    private readonly IClientSelectionParser parser;

    public ClientStrategySelector(
        IEnumerable<IClientStrategy> strategies,
        IClientSelectionParser parser)
    {
        this.strategies = strategies.ToDictionary(s => s.Key, StringComparer.OrdinalIgnoreCase);
        this.parser = parser;
    }

    public int Run(string[] args)
    {
        var selectionResult = parser.Parse(args);
        if (!selectionResult.IsSuccess)
        {
            System.Console.Error.WriteLine(selectionResult.Error);
            return 1;
        }

        var selection = selectionResult.Value!;

        if (strategies.TryGetValue(selection.ClientKey, out var strategy)) return strategy.Run(selection.RestArgs);
        System.Console.Error.WriteLine(
            $"Неизвестный клиент: {selection.ClientKey}. Доступно: {string.Join(", ", strategies.Keys)}");
        return 1;

    }
}