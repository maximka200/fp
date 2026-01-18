using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface ITagsBuilder
{
    Result<IReadOnlyCollection<Tag>> Build(IEnumerable<string> words);
}