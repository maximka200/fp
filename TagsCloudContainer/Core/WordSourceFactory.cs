using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core;

public static class WordsSourceFactory
{
    public static Result<IWordsSource> Create(SourceSettings settings, IWordsSource[] sources)
    {
        var format = settings.Format.Trim();

        var source = sources.FirstOrDefault(s =>
            s.CanHandle(new SourceSettings(settings.Path, format)));

        return source is null ? 
            Result<IWordsSource>.Failure($"No suitable words source found for the given format: {format}") : Result<IWordsSource>.Success(source);
    }
}