using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients;

public sealed class RestArgStrategy : IArgStrategy
{
    public Result<ArgStep> Handle(string[]? args, int index, ParseContext ctx)
    {
        ctx.AddRest(args[index]);
        return Result<ArgStep>.Success(ArgStep.Consumed(1));
    }
}