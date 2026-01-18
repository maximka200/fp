using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IWordsFilter
{
    Result<bool> ShouldKeep(string word);
}