using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal class HelpFlagPreCheck(string flag) : IPreCheck
{
    public Result<Unit> Check(string[] args)
    {
        return args.Contains(flag) ? Result<Unit>.Failure(ConsoleOptionsParser.HelpErrorCode) 
            : Result<Unit>.Success(Unit.Value);
    }
}