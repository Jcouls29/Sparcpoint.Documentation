// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;
using Sparcpoint.Documentation.Sql;
using Sparcpoint.Documentation.Stubble;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

public static class BuildSqlCommand
{
    public static Command GetCommand()
    {
        var buildCommand = new Command("build-sql", "Build the SQL Commands into Markdown")
        {
            new Option<string>(new[] { "-t", "--template" }, GetDefaultTemplatePath, "Path to the template directory"),
            new Option<bool>("--no-index-page", "Do not generate an index page"),
            new Option<bool>(new[] { "-v", "--verbose" }, "Show detail output logging"),
            new Argument<string>("path", "Path to source directory or .dbproj"),
            new Argument<string>("output", "Path to output directory")
        };

        buildCommand.Handler = CommandHandler.Create<string, bool, bool, string, string>(BuildSqlCommand.Run);

        return buildCommand;
    }

    private static string GetDefaultTemplatePath()
    {
        string? directory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location ?? throw new Exception("Invalid entry assembly. Be sure to access this method from an appropriate EXE."));
        return Path.Combine(directory ?? @".\", @"Templates\Default");
    }

    private static async Task Run(string template, bool noIndexPage, bool verbose, string path, string output)
    {
        List<Type> SortedTypes = new List<Type>(new[]
        {
            typeof(CreateSchemaStatement),
            typeof(CreateTableStatement),
            typeof(CreateIndexStatement),
            typeof(CreateTypeTableStatement),
            typeof(CreateSequenceStatement),
            typeof(CreateViewStatement),
            typeof(CreateProcedureStatement),
            typeof(CreateTriggerStatement),
            typeof(CreateTypeStatement),
        });

        IServiceProvider provider = BuildDependencyRoot(template, output, noIndexPage);
        var sqlLoaderFactory = provider.GetRequiredService<IFileStructureLoaderFactory<TSqlStatement>>();
        var handler = provider.GetRequiredService<ISqlServerStatementHandler>();

        if (!Directory.Exists(output))
            Directory.CreateDirectory(output);

        IEnumerable<string> files = sqlLoaderFactory.FromPath(path).Load();

        TSql150Parser parser = new TSql150Parser(true);
        SqlScriptGenerator generator = CreateSqlGenerator();
        ISqlTree tree = new InMemorySqlTree();

        List<TSqlStatement> statements = new List<TSqlStatement>();
        foreach (var file in files)
        {
            using (var reader = new StreamReader(file))
            {
                TSqlScript script = (TSqlScript)parser.Parse(reader, out IList<ParseError> errors);

                foreach (var batch in script.Batches)
                    statements.AddRange(batch.Statements);
            }
        }

        var sortedStatements = statements
            .Where(s => handler.CanHandle(s))
            .OrderBy(s =>
            {
                return SortedTypes.IndexOf(s.GetType());
            })
            .ToArray();

        foreach (var statement in sortedStatements)
            handler.Handle(statement, tree, generator);

        var constraintHandler = provider.GetRequiredService<ISqlServerConstraintHandler>();
        foreach (var constraint in tree.GetDeferredConstraints())
            if (constraintHandler.CanHandle(constraint))
                constraintHandler.Handle(constraint, tree, generator);

        // NOTE: Tree is built at this point
        ISqlTreeRenderer renderer = provider.GetRequiredService<ISqlTreeRenderer>();
        await renderer.RenderAsync(tree);
    }

    private static SqlScriptGenerator CreateSqlGenerator()
    {
        return new Sql150ScriptGenerator(new SqlScriptGeneratorOptions
        {
            KeywordCasing = KeywordCasing.Uppercase,
            SqlEngineType = SqlEngineType.All,
            IncludeSemicolons = true
        });
    }

    private static IServiceProvider BuildDependencyRoot(string templateDirectory, string outputDirectory, bool noIndexPage)
    {
        IServiceCollection services = new ServiceCollection();
        
        services.AddDefaultFileLoaders<TSqlStatement, DefaultSqlFileStructureLoaderFactory>(outputDirectory);
        services.AddTemplateLoader(templateDirectory);
        services.AddStubble();
        services.AddSqlServerHandlers(!noIndexPage);

        return services.BuildServiceProvider();
    }
}

