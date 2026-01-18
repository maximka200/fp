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

    public FontFamily ResolveOrDefault(FontFamily fallback)
    {
        var result = Resolve();
        return result.IsSuccess ? result.Value : fallback;
    }
    
    private static Result<FontFamily> Find(string name)
    {
        var font = SystemFonts.Collection.Families
            .FirstOrDefault(f => string.Equals(f.Name, name, StringComparison.OrdinalIgnoreCase));

        return Result<FontFamily>.Success(font);
    }
}