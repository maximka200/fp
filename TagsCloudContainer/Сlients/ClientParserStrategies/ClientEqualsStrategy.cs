using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients;

public class ClientEqualsStrategy : IArgStrategy
{
    private static readonly IReadOnlyDictionary<string, Action<string, ParseContext>> Map =
        new Dictionary<string, Action<string, ParseContext>>(StringComparer.OrdinalIgnoreCase)
        {
            [ClientSelectionParser.ClientFlag] =
                static (value, ctx) => ctx.SetClientKey(value)
        };

    public Result<ArgStep> Handle(string[] args, int index, ParseContext ctx)
    {
        var token = args[index];
        
        var parts = token.Split('=', 2);
        if (parts.Length != 2)
            return Result<ArgStep>.Success(ArgStep.Unhandled);

        var key = parts[0];
        var value = parts[1];

        if (!Map.TryGetValue(key, out var handler))
            return Result<ArgStep>.Success(ArgStep.Unhandled);
        
        handler(value, ctx);

        return Result<ArgStep>.Success(ArgStep.Consumed(1));
    }
}