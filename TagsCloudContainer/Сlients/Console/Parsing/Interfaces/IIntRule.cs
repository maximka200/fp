using TagsCloudContainer.Result;
namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

public interface IIntRule
{
    string Label { get; }
    Result<int> Validate(int value);
}
