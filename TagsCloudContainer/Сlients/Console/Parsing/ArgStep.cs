namespace TagsCloudContainer.Сlients.Console.Parsing;

public abstract class ArgStep
{
    public static readonly ArgStep Unhandled = new UnhandledStep();
    public static ArgStep Consumed(int count) => new HandledStep(count);
    
    public abstract bool IsHandled { get; }
    
    public abstract int ConsumedCount { get; }
    
    public int NextIndex(int currentIndex) => currentIndex + ConsumedCount;

    private sealed class UnhandledStep : ArgStep
    {
        public override bool IsHandled => false;
        public override int ConsumedCount => 0;
    }

    private sealed class HandledStep(int consumed) : ArgStep
    {
        public override bool IsHandled => true;
        public override int ConsumedCount => consumed;
    }
}