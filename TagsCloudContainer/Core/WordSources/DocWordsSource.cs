using NPOI.HWPF;
using NPOI.HWPF.Extractor;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.WordSources;

public class DocWordsSource : IWordsSource
{
    public string Format => "doc";

    public bool CanHandle(SourceSettings settings) =>
        settings.Format.Equals(Format, StringComparison.InvariantCultureIgnoreCase);

    public Result<IEnumerable<string>> GetWords(string path)
    {
        try
        {
            using var fs = File.OpenRead(path);
            var doc = new HWPFDocument(fs);
            var extractor = new WordExtractor(doc);

            var text = extractor.Text;
            return Result<IEnumerable<string>>.Success(WordTokenizer.Tokenize(text));
        }
        catch (Exception e)
        {
            return Result<IEnumerable<string>>.Failure($"Failed to read words from {Format} file: {e.Message}");
        }
    }
}