using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

public interface IArgConsoleStrategy
{
    Result<ArgStep> Handle(string[] args, int index, IDictionary<string, string?> flags);
}