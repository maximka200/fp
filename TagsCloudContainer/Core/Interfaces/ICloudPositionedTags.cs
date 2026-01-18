using SixLabors.Fonts;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface ICloudPositionedTags 
{
    Result<IEnumerable<PositionedTag>> GetPositionedTags(IEnumerable<Tag> tags, float minFontSize, float maxFontSize,
        bool invertSizeByFrequency, FontFamily ff);
}