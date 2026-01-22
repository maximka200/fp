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

    public Result<GenerationContext> ReadWords(IWordsReader reader) =>
        reader.Read(request)
            .Bind(w =>
            {
                words = w;
                return Result<GenerationContext>.Success(this);
            });

    public Result<GenerationContext> Preprocess(IWordsPreprocessor preprocessor) =>
        preprocessor.Process(words)
            .Bind(w =>
            {
                words = w;
                return Result<GenerationContext>.Success(this);
            });

    public Result<GenerationContext> BuildTags(ITagsBuilder builder) =>
        builder.Build(words)
            .Bind(t =>
            {
                tags = t;
                return Result<GenerationContext>.Success(this);
            });

    public Result<GenerationContext> Layout(ILayoutService layout)
    {
        if (tags.Count != 0)
            return layout.Layout(request, tags)
                .Bind(p =>
                {
                    positionedTags = p;
                    return Result<GenerationContext>.Success(this);
                });
        positionedTags = Array.Empty<PositionedTag>();
        return Result<GenerationContext>.Success(this);
    }


    public Result<GenerationContext> Render(ICloudRenderer renderer) =>
        renderer.Render(request, positionedTags)
            .Bind(img =>
            {
                image = img;
                return Result<GenerationContext>.Success(this);
            });

    public Result<GenerationContext> Save(IImageSaver saver)
    {
        if (image is null)
            return Result<GenerationContext>.Failure("Image not generated");

        using var img = image;
        image = null;
        
        return saver.Save(request, img)
            .Map(_ => this); 
    }
}
