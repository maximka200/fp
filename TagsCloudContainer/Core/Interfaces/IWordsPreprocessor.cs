using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsPreprocessor
{
    Result<IEnumerable<string>> Process(IEnumerable<string> words);
}