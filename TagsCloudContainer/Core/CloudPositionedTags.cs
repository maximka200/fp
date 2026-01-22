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

    public Result<IEnumerable<PositionedTag>> GetPositionedTags(
        IEnumerable<Tag> tags,
        float minFontSize,
        float maxFontSize,
        bool invertSizeByFrequency,
        FontFamily ff)
    {
        var tagList = tags.ToList();

        if (!tagList.Any())
            return Result<IEnumerable<PositionedTag>>.Failure("Tag list is empty.");

        return FrequencyRange.Get(tagList)
            .Bind(freq =>
                FontRange.Normalize(minFontSize, maxFontSize)
                    .Map(font =>
                    {
                        var (minFreq, maxFreq) = freq;
                        var (minFont, maxFont) = font;

                        var strategy = Strategies[invertSizeByFrequency];

                        return strategy.Order(tagList)
                            .Select(tag =>
                            {
                                var fontSize = GetFontSize(
                                    tag.Frequency,
                                    minFont,
                                    maxFont,
                                    minFreq,
                                    maxFreq,
                                    strategy);

                                var size = tagSizeCalculator.GetSize(tag, fontSize, ff);
                                var rect = cloudLayouter.PutNextRectangle(size);

                                return new PositionedTag(tag, rect, fontSize);
                            })
                            .ToList()
                            .AsEnumerable();
                    }));
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
