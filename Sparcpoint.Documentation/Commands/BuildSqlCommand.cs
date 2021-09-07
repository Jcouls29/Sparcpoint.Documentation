// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        IServiceProvider provider = BuildDependencyRoot(template, output, noIndexPage, verbose);
        var sqlLoaderFactory = provider.GetRequiredService<IFileStructureLoaderFactory<TSqlStatement>>();
        var handler = provider.GetRequiredService<ISqlServerStatementHandler>();
        var logger = provider.GetRequiredService<ILogger<_BuildSqlCommand>>();

        if (!Directory.Exists(output))
        {
            logger.LogTrace("Creating directory '{Directory}'...", output);
            Directory.CreateDirectory(output);
            logger.LogInformation("Created directory '{Directory}'", output);
        }

        IEnumerable<string> files = sqlLoaderFactory.FromPath(path).Load();
        logger.LogInformation("Loaded {FileCount} SQL Files.", files.Count());

        TSql150Parser parser = new TSql150Parser(true);
        SqlScriptGenerator generator = CreateSqlGenerator();
        ISqlTree tree = new InMemorySqlTree();

        List<TSqlStatement> statements = new List<TSqlStatement>();
        foreach (var file in files)
        {
            logger.LogTrace("\tSQL File Loaded: {File}", file);

            using (var reader = new StreamReader(file))
            {
                TSqlScript script = (TSqlScript)parser.Parse(reader, out IList<ParseError> errors);

                foreach (var batch in script.Batches)
                {
                    if (verbose)
                    {
                        generator.GenerateScript(batch, out string batchOutput);
                        logger.LogTrace("\t\t{Sql}", batchOutput);
                    }

                    statements.AddRange(batch.Statements);
                }
                    
            }
        }

        var unhandledStatements = statements
            .Where(s => !handler.CanHandle(s))
            .ToArray();
        
        foreach(var statementType in unhandledStatements.Select(s => s.GetType()).Distinct())
        {
            logger.LogWarning("WARNING: Cannot handle statement of type '{Type}'", statementType.Name);
        }

        var sortedStatements = statements
            .Except(unhandledStatements)
            .OrderBy(s =>
            {
                return SortedTypes.IndexOf(s.GetType());
            })
            .ToArray();

        logger.LogInformation("Total Parsed Sql Statements: {SupportedCount}/{TotalCount}", sortedStatements.Length, statements.Count());
        logger.LogInformation("Beginning Processing of Statements... {Count}", sortedStatements.Length);

        int iStatement = 1;
        foreach (var statement in sortedStatements)
        {
            logger.LogTrace("\tStatement {Index}:", iStatement);
            if (verbose)
            {
                generator.GenerateScript(statement, out string statementOutput);
                logger.LogTrace("\t\t{Sql}", statementOutput);
            }

            handler.Handle(statement, tree, generator);
            iStatement++;
        }

        var deferredConstraints = tree.GetDeferredConstraints();

        logger.LogInformation("Beginning Processing of Constraints... {Count}", deferredConstraints.Count());
        var constraintHandler = provider.GetRequiredService<ISqlServerConstraintHandler>();
        foreach (var constraint in deferredConstraints)
        {
            if (constraintHandler.CanHandle(constraint))
            {
                if (verbose)
                {
                    generator.GenerateScript(constraint.Constraint, out string constraintOutput);
                    logger.LogTrace("\t{TableName} => {Sql}", constraint.Table?.Identifier, constraintOutput);
                }

                constraintHandler.Handle(constraint, tree, generator);
            } else
            {
                logger.LogWarning("\tWARNING: Cannot handle constraint of type: {Type}", constraint.Constraint?.GetType()?.Name ?? "Unknown");
            }
        }

        // NOTE: Tree is built at this point
        ISqlTreeRenderer renderer = provider.GetRequiredService<ISqlTreeRenderer>();

        logger.LogInformation("Beginning Rendering of Sql Models...");
        await renderer.RenderAsync(tree);
        logger.LogInformation("Build Complete!");
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

    private static IServiceProvider BuildDependencyRoot(string templateDirectory, string outputDirectory, bool noIndexPage, bool verbose = false)
    {
        IServiceCollection services = new ServiceCollection();
        
        services.AddDefaultFileLoaders<TSqlStatement, DefaultSqlFileStructureLoaderFactory>(outputDirectory);
        services.AddTemplateLoader(templateDirectory);
        services.AddStubble();
        services.AddSqlServerHandlers(!noIndexPage);

        services.AddLogging(configure => configure.AddConsole());
        services.Configure<LoggerFilterOptions>(options =>
        {
            options.MinLevel = verbose ? LogLevel.Trace : LogLevel.Information;
        });

        return services.BuildServiceProvider();
    }

    private class _BuildSqlCommand { }
}

