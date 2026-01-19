using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Domains;
using TagsCloudContainer.Сlients.Domains;
using TagsCloudContainer.Сlients.Interfaces;
using Size = SixLabors.ImageSharp.Size;

namespace TagsCloudContainer.Сlients.Console;

public class ConsoleClient(ITagCloudGeneratorFactory generatorFactory) : IClient
{
    public int Run(string[] args)
    {
        var parseResult = ConsoleOptionsParser.Parse(args);

        if (!parseResult.IsSuccess)
        {
            if (parseResult.Error != ConsoleOptionsParser.HelpErrorCode)
                System.Console.WriteLine(parseResult.Error);

            PrintUsage();
            return 1;
        }

        var o = parseResult.Value!;

        var stopWordsExists = File.Exists(o.StopWordsPath);
        if (!stopWordsExists)
        {
            System.Console.WriteLine($"Файл стоп-слов не найден: {o.StopWordsPath}");
            return 1;
        }

        var settings = new TagCloudRuntimeSettings(
            CenterX: o.CenterX,
            CenterY: o.CenterY,
            StopWordsPath: o.StopWordsPath
        );

        var generator = generatorFactory.Create(settings);
        var generatingResult = generator.Generate(BuildRequest(o));

        if (!generatingResult.IsSuccess)
        {
            System.Console.WriteLine("Не удалось сгенерировать облако тегов:");
            System.Console.WriteLine(generatingResult.Error);
            return 1;
        }

        System.Console.WriteLine($"Облако тегов успешно сохранено в файл: {o.OutputPath}");
        return 0;
    }

    private static TagCloudGenerationRequest BuildRequest(ConsoleOptions o) =>
        new()
        {
            SourceSettings = new SourceSettings(o.InputPath, o.SourceType),
            LayoutSettings = new LayoutSettings
            {
                ImageSize = new Size(o.Width, o.Height),
                MinFontSize = o.MinFontSize,
                MaxFontSize = o.MaxFontSize
            },
            OutputPath = o.OutputPath,
            OutputFormat = o.OutputFormat,
            TextColor = o.TextColor,
            BackgroundColor = o.BackgroundColor,
            Font = o.Font,
            Desc = o.Desc
        };

    private static void PrintUsage()
    {
        System.Console.WriteLine("Console client flags:");
        System.Console.WriteLine("  --input <path> [--output cloud.png]");
        System.Console.WriteLine("  [--width 800] [--height 800] [--center-x width/2] [--center-y height/2]");
        System.Console.WriteLine("  [--stop-words ./stop-words.txt]");
        System.Console.WriteLine("  [--min-font 10] [--max-font 60] [--source-type txt]");
        System.Console.WriteLine("  [--bg #ffffff] [--fg #000000] [--font-family monospace|\"Arial\"|./fonts/Roboto.ttf]");
        System.Console.WriteLine("  [--format png|jpg|jpeg|bmp]");
        System.Console.WriteLine("  [--desc false|true]");
    }
}
