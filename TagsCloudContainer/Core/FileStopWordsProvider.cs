using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class FileStopWordsProvider(IWordNormalizer normalizer, string? stopWordsPath) : IStopWordsProvider
{
    private Lazy<Result<ISet<string>>> LazyStopWords => new(LoadStopWords);
    
    public Result<ISet<string>> GetStopWords() => LazyStopWords.Value;

    private Result<ISet<string>> LoadStopWords()
    {
        if (string.IsNullOrEmpty(stopWordsPath) || !File.Exists(stopWordsPath))
            Result<ISet<string>>.Failure($"Reading error: {stopWordsPath}");

        return Result<ISet<string>>.Success(
            File
            .ReadAllLines(stopWordsPath)
            .Select(normalizer.Normalize)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet()
            );
    }
}