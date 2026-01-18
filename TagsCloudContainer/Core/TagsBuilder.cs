using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public class TagsBuilder(IWordFrequencyAnalyzer analyzer) : ITagsBuilder
{
    private readonly IWordFrequencyAnalyzer analyzer = analyzer ?? throw new ArgumentNullException(nameof(analyzer));

    public Result<IReadOnlyCollection<Tag>> Build(IEnumerable<string> words)
    {
        var freq = analyzer.GetFrequencies(words);
        return Result<IReadOnlyCollection<Tag>>.Success(freq.Select(x => new Tag(x.Key, x.Value)).ToList());
    }
}
