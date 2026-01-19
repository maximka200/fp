using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;

public class EqualsFlagStrategy : IArgConsoleStrategy
{
    public Result<ArgStep> Handle(string[] args, int index, IDictionary<string, string?> flags)
    {
        var token = args[index];
        var parts = token.Split('=', 2);

        if (parts.Length != 2)
            return Result<ArgStep>.Success(ArgStep.Unhandled);

        var key = parts[0];
        var value = parts[1];

        var r = FlagStore.Put(flags, key, value);
        return !r.IsSuccess ? Result<ArgStep>.Failure(r.Error!) : Result<ArgStep>.Success(ArgStep.Consumed(1));
    }
}