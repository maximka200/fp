using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.OutputFormats;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IImageSaver
{
    Result<Unit> Save(TagCloudGenerationRequest request, Image<Rgba32> image);
}