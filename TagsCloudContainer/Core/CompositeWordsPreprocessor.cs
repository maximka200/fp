using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class CompositeWordsPreprocessor(
    IWordNormalizer normalizer,
    IEnumerable<IWordsFilter> filters) : IWordsPreprocessor
{
    public Result<IEnumerable<string>> Process(IEnumerable<string> words)
    {
        var result = new List<string>();

        foreach (var word in words.Select(normalizer.Normalize))
        {
            if (string.IsNullOrWhiteSpace(word))
                continue;

            var keepResult = ShouldKeepWord(word);

            if (!keepResult.IsSuccess)
                return Result<IEnumerable<string>>.Failure(
                    keepResult.Error ?? Result<IEnumerable<string>>.UnknownError);

            if (keepResult.Value)
                result.Add(word);
        }

        return Result<IEnumerable<string>>.Success(result);
    }
    
    private Result<bool> ShouldKeepWord(string word)
    {
        foreach (var filter in filters)
        {
            var keepResult = filter.ShouldKeep(word);

            if (!keepResult.IsSuccess)
                return Result<bool>.Failure(
                    keepResult.Error ?? Result<bool>.UnknownError);

            if (!keepResult.Value)
                return Result<bool>.Success(false);
        }

        return Result<bool>.Success(true);
    }
}
