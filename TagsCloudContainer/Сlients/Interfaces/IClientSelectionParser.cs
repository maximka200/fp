using TagsCloudContainer.Result;
using TagsCloudContainer.Сlients.Domains;

namespace TagsCloudContainer.Сlients.Interfaces;

public interface IClientSelectionParser
{
    Result<ClientSelection> Parse(string[] args);
}