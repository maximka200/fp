using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public static class WordsSourceFactory
{
    public static Result<IWordsSource> Create(SourceSettings settings, IWordsSource[] sources)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var format = settings.Format.Trim();

        var source = sources.FirstOrDefault(s =>
            s.CanHandle(new SourceSettings(settings.Path, format)));

        if (source is null)
            return Result<IWordsSource>.Failure($"No suitable words source found for the given format: {format}");

        return Result<IWordsSource>.Success(source);
    }
}