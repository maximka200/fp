using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class StopWordsFilter(IStopWordsProvider stopWordsProvider) : IWordsFilter
{
    public Result<bool> ShouldKeep(string word) =>
        stopWordsProvider.GetStopWords()
            .Map(stopWords => !stopWords.Contains(word));
}