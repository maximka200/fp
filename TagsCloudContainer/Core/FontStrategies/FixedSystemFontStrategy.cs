using SixLabors.Fonts;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.FontStrategies;

public abstract class FixedSystemFontStrategy : IFontChoiceStrategy
{
    private readonly Lazy<Result<FontFamily>> lazy;

    protected FixedSystemFontStrategy()
        => lazy = new Lazy<Result<FontFamily>>(() => Find(SystemFontName), isThreadSafe: true);

    public abstract string Key { get; }
    protected abstract string SystemFontName { get; }
    
    public Result<FontFamily> Resolve() => lazy.Value;
    
    private static Result<FontFamily> Find(string name)
    {
        try
        {
            var font = SystemFonts.Collection.Families
                .First(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));
            return Result<FontFamily>.Success(font);
        }
        catch (Exception e)
        {
            return Result<FontFamily>.Failure(e.Message);
        }
    }
}