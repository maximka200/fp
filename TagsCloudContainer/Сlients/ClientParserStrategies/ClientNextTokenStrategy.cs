using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients;

public sealed class ClientNextTokenStrategy : IArgStrategy
{
    private static readonly IReadOnlyDictionary<string, Func<string[], int, ParseContext, Result<ArgStep>>> Map =
        new Dictionary<string, Func<string[], int, ParseContext, Result<ArgStep>>>(
            StringComparer.OrdinalIgnoreCase)
        {
            [ClientSelectionParser.ClientFlag] = static (args, index, ctx) =>
            {
                if (index + 1 >= args.Length)
                {
                    return Result<ArgStep>.Failure(
                        $"Ожидалось значение после {ClientSelectionParser.ClientFlag}");
                }

                var value = args[index + 1];
                ctx.SetClientKey(value);
                return Result<ArgStep>.Success(ArgStep.Consumed(2));
            }
        };

    public Result<ArgStep> Handle(string[]? args, int index, ParseContext ctx)
    {
        var token = args[index];

        if (!Map.TryGetValue(token, out var handler))
            return Result<ArgStep>.Success(ArgStep.Unhandled);

        return handler(args, index, ctx);
    }
}