using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

internal interface IPreCheck
{
    Result<Unit> Check(string[] args);
}