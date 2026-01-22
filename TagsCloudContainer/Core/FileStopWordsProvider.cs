using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class FileStopWordsProvider(IWordNormalizer normalizer, string? stopWordsPath) : IStopWordsProvider
{
    private Lazy<Result<ISet<string>>> LazyStopWords => new(LoadStopWords);
    
    public Result<ISet<string>> GetStopWords() => LazyStopWords.Value;

    private Result<ISet<string>> LoadStopWords()
    {
        return ValidatePath(stopWordsPath)
            .Bind(ReadAllLines)
            .Map(lines =>
            {
                ISet<string> set = lines
                    .Select(normalizer.Normalize)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .ToHashSet();

                return set;
            });
    }
    
    private static Result<string> ValidatePath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result<string>.Failure("Stop words path is empty.");

        return !File.Exists(path) ? Result<string>.Failure($"Stop words file not found: {path}") : Result<string>.Success(path);
    }

    private static Result<string[]> ReadAllLines(string path)
    {
        try
        {
            return Result<string[]>.Success(File.ReadAllLines(path));
        }
        catch (Exception ex)
        {
            return Result<string[]>.Failure(ex.Message);
        }
    }
}
