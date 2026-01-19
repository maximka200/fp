using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients;

public interface IArgStrategy
{
    Result<ArgStep> Handle(string[]? args, int index, ParseContext ctx);
}
