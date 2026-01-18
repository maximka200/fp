using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface ITagCloudGenerator
{
    Result<GenerationContext> Generate(TagCloudGenerationRequest request);
}