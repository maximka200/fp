namespace TagsCloudContainer.Сlients.Console.Parsing;

public abstract class ArgStep
{
    public static readonly ArgStep Unhandled = new UnhandledStep();
    public static ArgStep Consumed(int count) => new HandledStep(count);
    
    public abstract bool IsHandled { get; }

    protected abstract int ConsumedCount { get; }
    
    public int NextIndex(int currentIndex) => currentIndex + ConsumedCount;

    private class UnhandledStep : ArgStep
    {
        public override bool IsHandled => false;
        protected override int ConsumedCount => 0;
    }

    private  class HandledStep(int consumed) : ArgStep
    {
        public override bool IsHandled => true;
        protected override int ConsumedCount => consumed;
    }
}