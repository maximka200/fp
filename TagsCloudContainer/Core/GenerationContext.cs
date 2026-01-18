using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class GenerationContext
{
    private readonly TagCloudGenerationRequest request;
    private IEnumerable<string> words = Array.Empty<string>();
    private IReadOnlyCollection<Tag> tags = Array.Empty<Tag>();
    private IReadOnlyCollection<PositionedTag> positionedTags = Array.Empty<PositionedTag>();
    private Image<Rgba32>? image;
    
    private GenerationContext(TagCloudGenerationRequest request) =>
        this.request = request ?? throw new ArgumentNullException(nameof(request));

    public static Result<GenerationContext> Start(TagCloudGenerationRequest request) 
        => Result<GenerationContext>.Success(new(request));

    public Result<GenerationContext> ReadWords(IWordsReader reader)
    {
        var readerResult = reader.Read(request);
        if (!readerResult.IsSuccess)
            return Result<GenerationContext>.Failure(readerResult.Error!);

        words = readerResult.Value!;
        return Result<GenerationContext>.Success(this);
    }

    public Result<GenerationContext> Preprocess(IWordsPreprocessor preprocessor)
    {
        var wordsResult = preprocessor.Process(words);
        if (!wordsResult.IsSuccess)
            return Result<GenerationContext>.Failure(wordsResult.Error!);

        words = wordsResult.Value!;
        return Result<GenerationContext>.Success(this);
    }

    public Result<GenerationContext> BuildTags(ITagsBuilder builder)
    {
        var tagsResult = builder.Build(words);
        if (!tagsResult.IsSuccess)
            return Result<GenerationContext>.Failure(tagsResult.Error!);

        tags = tagsResult.Value!;
        return Result<GenerationContext>.Success(this);
    }

    public Result<GenerationContext> Layout(ILayoutService layout)
    {
        if (tags.Count == 0)
            positionedTags = Array.Empty<PositionedTag>();
        else
        {
            var layoutResult = layout.Layout(request, tags);
            if (!layoutResult.IsSuccess)
                return Result<GenerationContext>.Failure(layoutResult.Error!);

            positionedTags = layoutResult.Value!;
        }

        return Result<GenerationContext>.Success(this);
    }

    public Result<GenerationContext> Render(ICloudRenderer renderer)
    {
        var imageResult = renderer.Render(request, positionedTags);
        if (!imageResult.IsSuccess)
            return Result<GenerationContext>.Failure(imageResult.Error!);

        image = imageResult.Value!;
        return Result<GenerationContext>.Success(this);
    }

    public Result<GenerationContext> Save(IImageSaver saver)
    {
        if (image is null)
            return Result<GenerationContext>.Failure("Image not generated");

        var savingResult = saver.Save(request, image);
        if (!savingResult.IsSuccess)
            return Result<GenerationContext>.Failure(savingResult.Error!);

        image.Dispose();
        image = null;

        return Result<GenerationContext>.Success(this);
    }
}
