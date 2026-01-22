using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class ImageSaver(IOutputFormat[] sources): IImageSaver
{
    public Result<Unit> Save(TagCloudGenerationRequest request, Image<Rgba32> image)
    {
        return OutputFormatFactory
            .Create(request.OutputFormat, sources)
            .Bind(source =>
                source.SaveImage(request.OutputPath, image)
                    .Map(_ => Unit.Value)
            );
    }
}
