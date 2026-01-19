using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Console.Parsing.Interfaces;

public interface IFloatRule
{
    string Label { get; }
    Result<float> Validate(float value);
}