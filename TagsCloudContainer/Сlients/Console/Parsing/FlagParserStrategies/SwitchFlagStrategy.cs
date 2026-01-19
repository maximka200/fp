using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;

public class SwitchFlagStrategy : IArgConsoleStrategy
{
    public Result<ArgStep> Handle(string[] args, int index, IDictionary<string, string?> flags)
    {
        var token = args[index];

        if (!string.Equals(token, CliFlags.Desc, StringComparison.OrdinalIgnoreCase))
            return Result<ArgStep>.Success(ArgStep.Unhandled);

        var r = FlagStore.Put(flags, CliFlags.Desc, "true");
        return !r.IsSuccess ? Result<ArgStep>.Failure(r.Error!) 
            : Result<ArgStep>.Success(ArgStep.Consumed(1));
    }
}