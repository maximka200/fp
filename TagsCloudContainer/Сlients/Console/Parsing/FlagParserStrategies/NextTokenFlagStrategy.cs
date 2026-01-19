using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing.FlagParserStrategies;

public class NextTokenFlagStrategy : IArgConsoleStrategy
{
    public Result<ArgStep> Handle(string[] args, int index, IDictionary<string, string?> flags)
    {
        var key = args[index];

        if (index + 1 >= args.Length)
            return Result<ArgStep>.Failure($"Ожидалось значение после {key}");

        var value = args[index + 1];

        try
        {
            Ensure.NextTokenIsValue(value, key);
        }
        catch (Exception e)
        {
            return Result<ArgStep>.Failure(e.Message);
        }

        var r = FlagStore.Put(flags, key, value);
        return !r.IsSuccess ? Result<ArgStep>.Failure(r.Error!) 
            : Result<ArgStep>.Success(ArgStep.Consumed(2));
    }
}