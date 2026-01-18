using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface ICloudRenderer
{
    Result<Image<Rgba32>> Render(TagCloudGenerationRequest request, IReadOnlyCollection<PositionedTag> positionedTags);
}