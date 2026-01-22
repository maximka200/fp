using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class WordsReader(IEnumerable<IWordsSource> sources) : IWordsReader
{
    public Result<IEnumerable<string>> Read(TagCloudGenerationRequest request)
    {
        return WordsSourceFactory
            .Create(request.SourceSettings, sources.ToArray())
            .Bind(source => source.GetWords(request.SourceSettings.Path));
    }
}