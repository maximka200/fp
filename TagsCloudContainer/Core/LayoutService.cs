using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class LayoutService(ICloudPositionedTags layouter) : ILayoutService
{
    private readonly ICloudPositionedTags layouter = layouter ?? throw new ArgumentNullException(nameof(layouter));

    public Result<IReadOnlyCollection<PositionedTag>> Layout(TagCloudGenerationRequest request, IReadOnlyCollection<Tag> tags)
    {
        var ffResult = FontFamilyResolver.Resolve(request.Font);
        if (!ffResult.IsSuccess)
            return Result<IReadOnlyCollection<PositionedTag>>.Failure(ffResult.Error ?? Result<IReadOnlyCollection<PositionedTag>>.UnknownError);
        var ff = ffResult.Value;
        var settings = request.LayoutSettings;

        var posTagsResult = layouter
            .GetPositionedTags(tags, settings.MinFontSize, settings.MaxFontSize, request.Desc, ff);
        if (!posTagsResult.IsSuccess)
            return Result<IReadOnlyCollection<PositionedTag>>.Failure(posTagsResult.Error ?? Result<IReadOnlyCollection<PositionedTag>>.UnknownError);
        
        var posTags = posTagsResult.Value.ToList();
        return Result<IReadOnlyCollection<PositionedTag>>.Success(posTags);
    }
}