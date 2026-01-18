using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Ranges;

public class EmptyRange : MaybeRange
{
    public override Result<(int Min, int Max)> GetOrYieldBreak() => Result<(int Min, int Max)>.Failure("Range is empty");
}