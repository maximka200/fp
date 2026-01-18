using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsSource
{
    string Format { get; }
    bool CanHandle(SourceSettings settings);
    Result<IEnumerable<string>> GetWords(string path);
}