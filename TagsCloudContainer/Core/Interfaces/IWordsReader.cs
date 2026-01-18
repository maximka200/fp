using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsReader
{
    Result<IEnumerable<string>> Read(TagCloudGenerationRequest request);
}