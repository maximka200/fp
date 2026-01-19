using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing;

public static class FlagsParser
{
    private static readonly IArgConsoleStrategy[] Strategies =
    [
        new SwitchFlagStrategy(),
        new EqualsFlagStrategy(),
        new NextTokenFlagStrategy()
    ];

    public static Result<IReadOnlyDictionary<string, string?>> Parse(string[] args)
    {
        var flags = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        var i = 0;

        while (i < args.Length)
        {
            var arg = args[i];
            
            if (!arg.StartsWith("--", StringComparison.Ordinal))
                return Result<IReadOnlyDictionary<string, string?>>.Failure(
                    $"Ожидался флаг, но получено: '{arg}'");

            Result<ArgStep>? handled = null;

            foreach (var strategy in Strategies)
            {
                var r = strategy.Handle(args, i, flags);

                if (!r.IsSuccess)
                    return Result<IReadOnlyDictionary<string, string?>>.Failure(r.Error!);

                if (!r.Value.IsHandled) continue;
                handled = r;
                break;
            }

            if (handled is null)
                return Result<IReadOnlyDictionary<string, string?>>.Failure(
                    $"Неизвестный флаг: {arg}");

            i = handled.Value!.NextIndex(i);
        }

        return Result<IReadOnlyDictionary<string, string?>>.Success(flags);
    }
}