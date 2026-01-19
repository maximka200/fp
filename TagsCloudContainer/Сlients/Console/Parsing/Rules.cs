using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Console.Parsing;

internal class PositiveIntRule(string label) : IIntRule
{
    public string Label => label;

    public Result<int> Validate(int value)
    {
        return value > 0
            ? Result<int>.Success(value)
            : Result<int>.Failure($"Некорректный {label}: {value}");
    }
}

internal class NonNegativeIntRule(string label) : IIntRule
{
    public string Label => label;

    public Result<int> Validate(int value)
    {
        return value >= 0
            ? Result<int>.Success(value)
            : Result<int>.Failure($"Некорректный {label}: {value}");
    }
}
internal class PositiveFloatRule(string label) : IFloatRule
{
    public string Label => label;

    public Result<float> Validate(float value)
    {
        return value > 0
            ? Result<float>.Success(value)
            : Result<float>.Failure($"Некорректный {label}: {value}");
    }
}
