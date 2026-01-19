using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Domains;
using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients;

public class ClientSelectionParser : IClientSelectionParser
{
    public const string ClientFlag = "--client";

    private static readonly IArgStrategy[] Strategies =
    [
        new ClientEqualsStrategy(),
        new ClientNextTokenStrategy(),
        new RestArgStrategy()
    ];

    public Result<ClientSelection> Parse(string[]? args)
    {
        if (args is null)
            return Result<ClientSelection>.Failure("args is null");

        var ctx = new ParseContext(args.Length);
        var i = 0;

        while (i < args.Length)
        {
            var result = Strategies.Aggregate(
                Result<ArgStep>.Success(ArgStep.Unhandled),
                (acc, strategy) =>
                {
                    if (!acc.IsSuccess)
                        return acc;

                    return acc.Value != ArgStep.Unhandled ? acc : strategy.Handle(args, i, ctx);
                });

            if (!result.IsSuccess)
                return Result<ClientSelection>.Failure(result.Error!);

            i = result.Value!.NextIndex(i);
        }
        
        var buildR = ctx.Build();
        return !buildR.IsSuccess ? Result<ClientSelection>.Failure(buildR.Error!) 
            : Result<ClientSelection>.Success(buildR.Value);
    }
}