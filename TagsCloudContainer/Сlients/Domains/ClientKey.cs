using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients.Domains;

public static class ClientKey
{
    public static Result<string> Parse(string raw)
    {
        var trimmed = string.Concat(raw).Trim();

        return string.IsNullOrEmpty(trimmed)
            ? Result<string>.Failure("Пустое значение флага")
            : Result<string>.Success(trimmed);
    }
}