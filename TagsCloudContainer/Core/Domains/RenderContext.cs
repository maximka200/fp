using SixLabors.Fonts;
using SixLabors.ImageSharp;

namespace TagsCloudContainer.Core.Domains;

public record RenderContext(
    FontFamily FontFamily,
    float MinFontSize,
    float MaxFontSize,
    int MinFreq,
    int MaxFreq,
    Color TextColor
);