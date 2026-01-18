using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class StopWordsFilter(IStopWordsProvider stopWordsProvider) : IWordsFilter
{
    public Result<bool> ShouldKeep(string word)
    {
        var stopWordsResult = stopWordsProvider.GetStopWords();
        if (!stopWordsResult.IsSuccess)
            return Result<bool>.Failure(stopWordsResult.Error ?? Result<bool>.UnknownError);
        var stopWords = stopWordsResult.Value;
        var shouldKeep = !stopWords.Contains(word);
        return Result<bool>.Success(shouldKeep);
    }
}