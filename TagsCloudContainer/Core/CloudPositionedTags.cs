using SixLabors.Fonts;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.FrequencySizingStrategies;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class CloudPositionedTags(
    ICircularCloudLayouterWrapper cloudLayouter,
    ITagSizeCalculator tagSizeCalculator)
    : ICloudPositionedTags
{
    private static readonly IReadOnlyDictionary<bool, IFrequencySizingStrategy> Strategies =
        new IFrequencySizingStrategy[]
        {
            new DirectFrequencySizingStrategy(),
            new InvertedFrequencySizingStrategy()
        }.ToDictionary(s => s.Inverted);

    public Result<IEnumerable<PositionedTag>> GetPositionedTags(IEnumerable<Tag> tags, float minFontSize,
        float maxFontSize, bool invertSizeByFrequency, FontFamily ff)
    {
        var tagList = tags.ToList();
        
        if (!tagList.Any())
            return Result<IEnumerable<PositionedTag>>.Failure("Tag list is empty.");

        var freqRangeResult = FrequencyRange.TryGet(tagList).GetOrYieldBreak();
        if (!freqRangeResult.IsSuccess)
            return Result<IEnumerable<PositionedTag>>.Failure(freqRangeResult.Error ?? Result<IEnumerable<PositionedTag>>.UnknownError);
        
        var (minFreq, maxFreq) = freqRangeResult.Value;
        var fontResult = FontRange.Normalize(minFontSize, maxFontSize);
        if (!fontResult.IsSuccess)
            return Result<IEnumerable<PositionedTag>>.Failure(fontResult.Error ?? Result<IEnumerable<PositionedTag>>.UnknownError);
        
        var (minFont, maxFont) = fontResult.Value;
        
        var strategy = Strategies[invertSizeByFrequency];
        
        var result = new List<PositionedTag>();
        foreach (var tag in strategy.Order(tagList))
        {
            var fontSize = GetFontSize(tag.Frequency, minFont, maxFont, minFreq, maxFreq, strategy);
            var size = tagSizeCalculator.GetSize(tag, fontSize, ff);
            var rect = cloudLayouter.PutNextRectangle(size);
            result.Add(new PositionedTag(tag, rect, fontSize));
        }
        
        return Result<IEnumerable<PositionedTag>>.Success(result);
    }

    private static float GetFontSize(int frequency, float minFontSize,
        float maxFontSize, int minFreq, int maxFreq,
        IFrequencySizingStrategy strategy)
    {
        var avg = (minFontSize + maxFontSize) / 2f;

        var normalized = Normalizer.Normalize(frequency, minFreq, maxFreq).OrElse(avg);
        return strategy.Scale(minFontSize, maxFontSize, normalized);
    }
}
