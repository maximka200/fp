using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class TagCloudGenerator(
    IWordsReader wordsReader,
    IWordsPreprocessor wordsPreprocessor,
    ITagsBuilder tagsBuilder,
    ILayoutService layoutService,
    ICloudRenderer renderer,
    IImageSaver saver)
    : ITagCloudGenerator
{
    public Result<GenerationContext> Generate(TagCloudGenerationRequest request)
    {
        return GenerationContext
            .Start(request)
            .Bind(ctx => ctx.ReadWords(wordsReader))
            .Bind(ctx => ctx.Preprocess(wordsPreprocessor))
            .Bind(ctx => ctx.BuildTags(tagsBuilder))
            .Bind(ctx => ctx.Layout(layoutService))
            .Bind(ctx => ctx.Render(renderer))
            .Bind(ctx => ctx.Save(saver));
    }
}
