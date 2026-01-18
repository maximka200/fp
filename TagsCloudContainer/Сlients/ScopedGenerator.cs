using Autofac;
using TagsCloudContainer.Core;
using TagsCloudContainer.Core.Interfaces;
using TagsCloudContainer.Result;

namespace TagsCloudContainer.Сlients;

public class ScopedGenerator(ILifetimeScope scope) : ITagCloudGenerator, IDisposable
{
    private readonly ITagCloudGenerator inner = scope.Resolve<ITagCloudGenerator>();

    public Result<GenerationContext> Generate(Core.Domains.TagCloudGenerationRequest request) => inner.Generate(request);

    public void Dispose() => scope.Dispose();
}