using SixLabors.Fonts;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IFontChoiceStrategy
{
    string Key { get; }
    Result<FontFamily> Resolve();
}
