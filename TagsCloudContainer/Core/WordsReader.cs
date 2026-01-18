using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class WordsReader(IEnumerable<IWordsSource> sources) : IWordsReader
{
    public Result<IEnumerable<string>> Read(TagCloudGenerationRequest request)
    {
        var sourceResult = WordsSourceFactory.Create(
            request.SourceSettings,
            sources.ToArray());
        if (!sourceResult.IsSuccess)
            return Result<IEnumerable<string>>.Failure(sourceResult.Error);
        
        var source = sourceResult.Value;
        
        var wordsResult = source.GetWords(request.SourceSettings.Path);
        if (!wordsResult.IsSuccess)
            return Result<IEnumerable<string>>.Failure(wordsResult.Error);
        var words = wordsResult.Value;
        
        return Result<IEnumerable<string>>.Success(words);
    }

}