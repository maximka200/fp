using SixLabors.ImageSharp;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Interfaces;

public interface IOutputFormat
{
    string Format { get; }
    bool CanHandle(string format);
    Result<Unit> SaveImage(string path, Image image);
}