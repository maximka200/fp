using TagsCloudContainer.Сlients.Interfaces;

namespace TagsCloudContainer.Сlients.Console;

public class ConsoleClientStrategy(IClient client) : IClientStrategy
{
    public string Key => "console";

    public int Run(string[] args)
    {
        return client.Run(args);
    }
}