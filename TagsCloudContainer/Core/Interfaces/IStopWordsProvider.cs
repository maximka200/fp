using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IStopWordsProvider
{
    Result<ISet<string>> GetStopWords();
}