using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface ILayoutService
{
    Result<IReadOnlyCollection<PositionedTag>> Layout(TagCloudGenerationRequest request, IReadOnlyCollection<Tag> tags);
}