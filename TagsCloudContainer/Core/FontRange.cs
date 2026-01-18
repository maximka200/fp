using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public static class FontRange
{
    public static Result<(float Min, float Max)> Normalize(float minFontSize, float maxFontSize)
    {
        if (maxFontSize <= 0)
            return Result<(float Min, float Max)>.Failure("Max font size must be positive.");
        if (minFontSize <= 0)
            return Result<(float Min, float Max)>.Failure("Min font size must be positive.");

        var min = Math.Min(minFontSize, maxFontSize);
        var max = Math.Max(minFontSize, maxFontSize);
        return Result<(float Min, float Max)>.Success((min, max));
    }
}