using Microsoft.Extensions.DependencyInjection;
using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultFileLoaders<TMarker, TMarkerFactory>(this IServiceCollection services, string outputDirectory)
            where TMarkerFactory : class, IFileStructureLoaderFactory<TMarker>
            => services
                .AddSingleton<IFileStructureLoaderFactory<TMarker>, TMarkerFactory>()
                .AddSingleton<IFileStructureLoaderFactory<Template>, DefaultTemplateStructureLoaderFactory>()
                .AddSingleton<IFileWriter, DefaultFileWriter>()
                .AddSingleton<IFileStructureWriter>((provider) => new FlatFileStructureWriter(provider.GetRequiredService<IFileWriter>(), outputDirectory))
            ;

        public static IServiceCollection AddTemplateLoader(this IServiceCollection services, string templateDirectory)
            => services.AddSingleton<ITemplateLoader>(sp => new FileNameMatchTemplateLoader(
                    sp.GetRequiredService<IFileStructureLoaderFactory<Template>>(),
                    templateDirectory)
                )
            ;
    }
}
