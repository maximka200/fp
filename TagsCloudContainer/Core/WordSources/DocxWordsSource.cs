using NPOI.XWPF.UserModel;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.WordSources;

public sealed class DocxWordsSource : IWordsSource
{
    public string Format => "docx";

    public bool CanHandle(SourceSettings settings) =>
        settings.Format.Equals(Format, StringComparison.InvariantCultureIgnoreCase);

    public Result<IEnumerable<string>> GetWords(string path)
    {
        try
        {
            using var fs = File.OpenRead(path);
            var doc = new XWPFDocument(fs);

            var text = string.Join(
                Environment.NewLine,
                doc.Paragraphs.Select(p => p.Text));

            return Result<IEnumerable<string>>.Success(WordTokenizer.Tokenize(text));
        }
        catch (Exception e)
        {
            return Result<IEnumerable<string>>.Failure($"Failed to read words from {Format} file: {e.Message}");
        }
    }
}