// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Newtonsoft.Json;
using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;
using Sparcpoint.Documentation.Sql;
using Sparcpoint.Documentation.Stubble;
using System.CommandLine;
using System.CommandLine.Invocation;

var buildCommand = new Command("build-sql", "Build the SQL Commands into Markdown")
    {
        new Option<string>(new[] { "-t", "--template" }, () => @".\Templates", "Path to the template directory"),
        new Option<bool>("--no-index-page", "Do not generate an index page"),
        new Option<bool>(new[] { "-v", "--verbose" }, "Show detail output logging"),
        new Argument<string>("path", "Path to source directory or .dbproj"),
        new Argument<string>("output", "Path to output directory")
    };
buildCommand.Handler = CommandHandler.Create<string, bool, bool, string, string>(BuildSql);

var rootCommand = new RootCommand
{
    buildCommand
};

await rootCommand.InvokeAsync(args);
Console.ReadKey();

static IServiceProvider BuildDependencyRoot(string templateDirectory)
{
    IServiceCollection services = new ServiceCollection();
    services.AddSingleton<IFileStructureLoaderFactory<TSqlStatement>, DefaultSqlFileStructureLoaderFactory>();
    services.AddSingleton<IFileStructureLoaderFactory<Template>, DefaultTemplateStructureLoaderFactory>();
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

    services.AddSingleton<ITemplateLoader>(sp => new FileNameMatchTemplateLoader(
        sp.GetRequiredService<IFileStructureLoaderFactory<Template>>(), 
        templateDirectory));
    services.AddSingleton<ITemplateProcessor<string>, StubbleTemplateProcessor>();
    return services.BuildServiceProvider();
}

static async Task BuildSql(string template, bool noIndexPage, bool verbose, string path, string output)
{
    IServiceProvider provider = BuildDependencyRoot(template);
    var sqlLoaderFactory = provider.GetRequiredService<IFileStructureLoaderFactory<TSqlStatement>>();
    var handler = provider.GetRequiredService<ISqlServerStatementHandler>();

    if (!Directory.Exists(output))
        Directory.CreateDirectory(output);

    IEnumerable<string> files = sqlLoaderFactory.FromPath(path).Load();

    TSql150Parser parser = new TSql150Parser(true);
    SqlScriptGenerator generator = CreateSqlGenerator();
    ISqlTree tree = new InMemorySqlTree();

    foreach(var file in files)
    {
        using (var reader  = new StreamReader(file))
        {
            TSqlScript script = (TSqlScript)parser.Parse(reader, out IList<ParseError> errors);
            
            foreach(var batch in script.Batches)
                foreach(var statement in batch.Statements)
                {
                    if (handler.CanHandle(statement))
                        await handler.HandleAsync(file, statement, tree, generator);
                }
        }
    }
}

static SqlScriptGenerator CreateSqlGenerator()
{
    return new Sql150ScriptGenerator(new SqlScriptGeneratorOptions
    {
        KeywordCasing = KeywordCasing.Uppercase,
        SqlEngineType = SqlEngineType.All,
        IncludeSemicolons = true
    });
}

