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

            foreach (var filter in filters)
            {
                var keepResult = filter.ShouldKeep(word);

                if (!keepResult.IsSuccess)
                    return Result<IEnumerable<string>>.Failure(
                        keepResult.Error ?? Result<IEnumerable<string>>.UnknownError);

                if (!keepResult.Value)
                    goto SkipWord;
            }

            result.Add(word);

            SkipWord: ;
        }

        return Result<IEnumerable<string>>.Success(result);
    }
}
