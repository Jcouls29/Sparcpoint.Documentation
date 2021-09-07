using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Stubble;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStubble(this IServiceCollection services)
            => services
                .AddSingleton<ITemplateProcessor<string>, StubbleTemplateProcessor>()
            ;
    }
}
