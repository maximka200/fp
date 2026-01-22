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
            var result = TryHandleStep(args, i, ctx);

            if (!result.IsSuccess)
                return Result<ClientSelection>.Failure(result.Error ?? Result<ClientSelection>.UnknownError);

            i = result.Value!.NextIndex(i);
        }

        return ctx.Build();
    }

    private static Result<ArgStep> TryHandleStep(string[] args, int index, ParseContext ctx)
    {
        return Strategies.Aggregate(
            Result<ArgStep>.Success(ArgStep.Unhandled),
            (acc, strategy) =>
            {
                if (!acc.IsSuccess)
                    return acc; 

                return acc.Value != ArgStep.Unhandled
                    ? acc
                    : strategy.Handle(args, index, ctx);
            }
        );
    }
}