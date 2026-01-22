using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;
namespace TagsCloudContainer.Core;

public static class OutputFormatFactory
{
    public static Result<IOutputFormat> Create(string format, IOutputFormat[] sources)
    {
        var normalized = Normalize(format);

        var source = sources.FirstOrDefault(d =>
            string.Equals(d.Format, normalized, StringComparison.OrdinalIgnoreCase));

        return source is null ? Result<IOutputFormat>.Failure($"Output format '{format}' is not support") : Result<IOutputFormat>.Success(source);
    }

    private static string Normalize(string format) =>
        format.Trim().TrimStart('.').ToLowerInvariant();
}