using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public static class FrequencyRange
{
    public static Result<(int min, int max)> Get(IReadOnlyList<Tag> tags)
    {
        if (tags.Count == 0)
            return Result<(int, int)>.Failure("Cannot calculate frequency range: tag list is empty.");

        var min = tags.Min(t => t.Frequency);
        var max = tags.Max(t => t.Frequency);

        return Result<(int min, int max)>.Success((min, max));
    }
}