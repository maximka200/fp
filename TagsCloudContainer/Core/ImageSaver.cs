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
        var sourceResult = OutputFormatFactory.Create(request.OutputFormat, sources);
        if (!sourceResult.IsSuccess)
            return Result<Unit>.Failure(sourceResult.Error);
        var source = sourceResult.Value;
        var saveResult = source.SaveImage(request.OutputPath, image);
        return !saveResult.IsSuccess ? Result<Unit>.Failure(saveResult.Error) : Result<Unit>.Success(Unit.Value);
    }
}
