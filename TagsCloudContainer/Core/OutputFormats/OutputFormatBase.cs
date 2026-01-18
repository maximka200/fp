using SixLabors.ImageSharp;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.OutputFormats;

public abstract class OutputSourceBase : IOutputFormat
{
    public abstract string Format { get; }

    public bool CanHandle(string format) =>
        string.Equals(Normalize(format), Format, StringComparison.OrdinalIgnoreCase);

    public Result<Unit> SaveImage(string path, Image image)
    {
        if (string.IsNullOrWhiteSpace(path))
            return Result<Unit>.Failure($"Output path is empty: {nameof(path)}");

        SaveImageInternal(image, path);
        
        return Result<Unit>.Success(Unit.Value);
    }

    protected abstract void SaveImageInternal(Image image, string path);

    private static string Normalize(string format) =>
        format.Trim().TrimStart('.').ToLowerInvariant();
}