using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class LayoutService(ICloudPositionedTags layouter) : ILayoutService
{
    private readonly ICloudPositionedTags layouter = layouter ?? throw new ArgumentNullException(nameof(layouter));

    public Result<IReadOnlyCollection<PositionedTag>> Layout(TagCloudGenerationRequest request, IReadOnlyCollection<Tag> tags)
    {
        return FontFamilyResolver.Resolve(request.Font)
            .Bind(ff =>
                layouter.GetPositionedTags(tags, 
                        request.LayoutSettings.MinFontSize, 
                        request.LayoutSettings.MaxFontSize, 
                        request.Desc, 
                        ff)
                    .Map(posTags => posTags.ToList() as IReadOnlyCollection<PositionedTag>)
            );
    }
}