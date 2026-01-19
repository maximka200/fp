using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal class EmptyArgsIsHelpPreCheck : IPreCheck
{
    public Result<Unit> Check(string[] args)
    {
        return args.Length == 0 ? Result<Unit>.Failure(ConsoleOptionsParser.HelpErrorCode) 
            : Result<Unit>.Success(Unit.Value);
    }
}