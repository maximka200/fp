using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.WordSources;

public class TxtWordsSource : IWordsSource
{
    public string Format => "txt";
    public bool CanHandle(SourceSettings settings) =>
        settings.Format.Equals(Format, StringComparison.InvariantCultureIgnoreCase);

    public Result<IEnumerable<string>> GetWords(string path)
    {
        try
        {
            return Result<IEnumerable<string>>.Success(
                File.ReadLines(path).Where(line => !string.IsNullOrWhiteSpace(line))
                );
        }
        catch (Exception e)
        {
            return Result<IEnumerable<string>>.Failure($"Failed to read words from {Format} file: {e.Message}");
        }
    }
}
