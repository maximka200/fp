using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Console.Parsing;
using TagsCloudContainer.Сlients.Console.Parsing.Interfaces;
using Color = SixLabors.ImageSharp.Color;

namespace TagsCloudContainer.Сlients.Console;

public static class ConsoleOptionsParser
{
    public const string HelpErrorCode = "help";

    private const int BaseWidth = 800;
    private const int BaseHeight = 800;
    private const string BaseOutput = "cloud.png";
    private const string BaseStopWordsPath = "stop-words.txt";
    private const string BaseFont = "arial";
    private static readonly Color BaseBc = Color.White;
    private static readonly Color BaseTc = Color.Black;

    private static readonly IPreCheck[] PreChecks =
    [
        new EmptyArgsIsHelpPreCheck(),
        new HelpFlagPreCheck("-h"),
        new HelpFlagPreCheck("--help"),
    ];

    public static Result<ConsoleOptions> Parse(string[]? args)
    {
        if (args is null)
            return Result<ConsoleOptions>.Failure("Args is null");
        
        foreach (var preCheck in PreChecks)
        {
            var result = preCheck.Check(args);
            if (!result.IsSuccess)
                return Result<ConsoleOptions>.Failure(result.Error ?? Result<ConsoleOptions>.UnknownError);
        }

        var flagsR = FlagsParser.Parse(args);
        if (!flagsR.IsSuccess)
            return Fail(flagsR);
        var flags = flagsR.Value!;
        var r = new FlagReader(flags);
        
        var inputPathR = r.RequirePath(CliFlags.Input, "Входной файл");
        if (!inputPathR.IsSuccess) return Fail(inputPathR);
        
        var outputPathR = r.GetString(CliFlags.Output, BaseOutput);
        if (!outputPathR.IsSuccess) return Fail(outputPathR);
        
        var widthR = r.GetInt(CliFlags.Width, BaseWidth, new PositiveIntRule(CliFlags.Width));
        if (!widthR.IsSuccess) return Fail(widthR);

        var heightR = r.GetInt(CliFlags.Height, BaseHeight, new PositiveIntRule(CliFlags.Height));
        if (!heightR.IsSuccess) return Fail(heightR);

        var centerXr = r.GetInt(CliFlags.CenterX, widthR.Value / 2,
            new NonNegativeIntRule(CliFlags.CenterX));
        if (!centerXr.IsSuccess) return Fail(centerXr);

        var centerYr = r.GetInt(CliFlags.CenterY, heightR.Value/ 2,
            new NonNegativeIntRule(CliFlags.CenterY));
        if (!centerYr.IsSuccess) return Fail(centerYr);
        
        var stopWordsPathR =
            r.GetString(CliFlags.StopWords,
                Path.Combine(AppContext.BaseDirectory, BaseStopWordsPath));
        if (!stopWordsPathR.IsSuccess) return Fail(stopWordsPathR);

        var stopWordsPath = Path.GetFullPath(stopWordsPathR.Value!);
        
        var minFontR = r.GetFloat(CliFlags.MinFont, 10f, new PositiveFloatRule(CliFlags.MinFont));
        if (!minFontR.IsSuccess) return Fail(minFontR);

        var maxFontR = r.GetFloat(CliFlags.MaxFont, 60f, new PositiveFloatRule(CliFlags.MaxFont));
        if (!maxFontR.IsSuccess) return Fail(maxFontR);

        var fontRangeR = Ensure.True(
            minFontR.Value <= maxFontR.Value,
            "Некорректные значения шрифтов: min > max"
        );
        if (!fontRangeR.IsSuccess)
            return Result<ConsoleOptions>.Failure(fontRangeR.Error!);
        
        var sourceTypeR = r.GetString(
            CliFlags.SourceType,
            SourceFormatSupport.FormatFromPath(inputPathR.Value!)
        );
        if (!sourceTypeR.IsSuccess) return Fail(sourceTypeR);

        SourceFormatSupport.EnsureFormatSupported(sourceTypeR.Value!);

        var bgR = r.GetColor(CliFlags.Bg, BaseBc);
        if (!bgR.IsSuccess) return Fail(bgR);

        var fgR = r.GetColor(CliFlags.Fg, BaseTc);
        if (!fgR.IsSuccess) return Fail(fgR);

        var outputFormatR = r.GetString(
            CliFlags.Format,
            OutputFormatSupport.FormatFromPath(outputPathR.Value!)
        );
        if (!outputFormatR.IsSuccess) return Fail(outputFormatR);

        OutputFormatSupport.EnsureFormatSupported(outputFormatR.Value!);

        var fontR = r.GetString(CliFlags.Font, BaseFont);
        if (!fontR.IsSuccess) return Fail(fontR);

        var invertR = r.GetBool(CliFlags.Desc, false);
        if (!invertR.IsSuccess) return Fail(invertR);

        return Result<ConsoleOptions>.Success(
            new ConsoleOptions(
                InputPath: inputPathR.Value!,
                OutputPath: outputPathR.Value!,
                Width: widthR.Value,
                Height: heightR.Value,
                CenterX: centerXr.Value,
                CenterY: centerYr.Value,
                StopWordsPath: stopWordsPath,
                MinFontSize: minFontR.Value,
                MaxFontSize: maxFontR.Value,
                SourceType: sourceTypeR.Value!,
                OutputFormat: outputFormatR.Value!,
                BackgroundColor: bgR.Value!,
                TextColor: fgR.Value!,
                Font: fontR.Value!,
                Desc: invertR.Value!
            )
        );
    }

    private static Result<ConsoleOptions> Fail<T>(Result<T> r) =>
        Result<ConsoleOptions>.Failure(r.Error ?? Result<ConsoleOptions>.UnknownError);
    
    // for tests
    public static bool TryParse(
        string[] args,
        out ConsoleOptions? options,
        out string error)
    {
        var result = Parse(args);

        if (!result.IsSuccess)
        {
            options = null;
            error = result.Error ?? string.Empty;
            return false;
        }

        options = result.Value!;
        error = string.Empty;
        return true;
    }

}
