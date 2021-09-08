using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Sql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerHandlers(this IServiceCollection services, bool includeIndexPage = true)
        {
            services.AddSingleton<ISqlServerStatementHandler, DefaultSqlServerStatementHandler>();

            // Adds all Statement Handlers automatically without
            // having to explicitly define them
            services.Scan(_ =>
            {
                _.FromAssemblyOf<ISqlServerStatementHandler>()
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(ISqlServerStatementHandler<>)))
                        .AsImplementedInterfaces();
            });

            services.AddSingleton<ISqlServerConstraintHandler, DefaultSqlServerConstraintHandler>();
            services.Scan(_ =>
            {
                _.FromAssemblyOf<ISqlServerConstraintHandler>()
                    .AddClasses(classes => classes
                        .AssignableTo(typeof(ISqlServerConstraintHandler<>)))
                        .AsImplementedInterfaces();
            });

            services.AddSingleton<ISqlTreeRenderer, DefaultTemplateSqlTreeRenderer>();

            services.Configure<TreeRendererOptions>((opt) =>
            {
                opt.RenderIndex = includeIndexPage;
            });

            return services;
        }
    }
}
