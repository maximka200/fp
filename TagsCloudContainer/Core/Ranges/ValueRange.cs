using TagsCloudContainer.Result;

namespace TagsCloudContainer.Core.Ranges;

public class ValueRange(int min, int max) : MaybeRange
{
    public override Result<(int Min, int Max)> GetOrYieldBreak() => Result<(int Min, int Max)>.Success((min, max));
}