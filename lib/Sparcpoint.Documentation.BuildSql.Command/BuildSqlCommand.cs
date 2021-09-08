using Microsoft.Extensions.DependencyInjection;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Sql;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.BuildSql
{
    public class BuildSqlCommand
    {
        private readonly BuildSqlCommandOptions _Options;
        private readonly IConsole _Console;

        public BuildSqlCommand(BuildSqlCommandOptions options, IConsole console)
        {
            _Options = options ?? throw new ArgumentNullException(nameof(options));
            _Console = console ?? throw new ArgumentNullException(nameof(console));
        }

        public async Task Run()
        {
            if (_Options == null)
                throw new ArgumentNullException(nameof(_Options));

            if (string.IsNullOrWhiteSpace(_Options.Template) || !Directory.Exists(_Options.Template))
                throw new Exception("Template directory is required.");

            if (string.IsNullOrWhiteSpace(_Options.Path) || !Directory.Exists(_Options.Template))
                throw new Exception("Source directory is required.");

            if (string.IsNullOrWhiteSpace(_Options.Output))
                throw new Exception("Output directory is required.");

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
            
            IServiceProvider provider = BuildDependencyRoot(_Options);
            var sqlLoaderFactory = provider.GetRequiredService<IFileStructureLoaderFactory<TSqlStatement>>();
            var handler = provider.GetRequiredService<ISqlServerStatementHandler>();

            if (!Directory.Exists(_Options.Output))
            {
                LogTrace($"Creating directory '{_Options.Output}'...");
                Directory.CreateDirectory(_Options.Output);
                LogInformation($"Created directory '{_Options.Output}'");
            }

            IEnumerable<string> files = sqlLoaderFactory.FromPath(_Options.Path).Load();
            LogInformation($"Loaded {files.Count()} SQL Files.");

            if (files.Count() == 0)
                throw new Exception("No SQL files found.");

            TSql150Parser parser = new TSql150Parser(true);
            SqlScriptGenerator generator = CreateSqlGenerator();
            ISqlTree tree = new InMemorySqlTree();

            List<TSqlStatement> statements = new List<TSqlStatement>();
            foreach (var file in files)
            {
                LogTrace($"\tSQL File Loaded: {file}");

                using (var reader = new StreamReader(file))
                {
                    TSqlScript script = (TSqlScript)parser.Parse(reader, out IList<ParseError> errors);

                    foreach (var batch in script.Batches)
                    {
                        if (_Options.Verbose)
                        {
                            generator.GenerateScript(batch, out string batchOutput);
                            LogTrace($"\t\t{batchOutput.Trim()}");
                        }

                        statements.AddRange(batch.Statements);
                    }
                }
            }

            var unhandledStatements = statements
                .Where(s => !handler.CanHandle(s))
                .ToArray();

            foreach (var statementType in unhandledStatements.Select(s => s.GetType()).Distinct())
            {
                LogWarning($"Cannot handle statement of type '{statementType.Name}'");
            }

            var sortedStatements = statements
                .Except(unhandledStatements)
                .OrderBy(s =>
                {
                    return SortedTypes.IndexOf(s.GetType());
                })
                .ToArray();

            if (sortedStatements.Length == 0)
                throw new Exception("No valid SQL scripts found. No processing will occur.");

            LogInformation($"Total Parsed Sql Statements: {sortedStatements.Length}/{statements.Count()}");
            LogInformation($"Beginning Processing of Statements... {sortedStatements.Length}");

            int iStatement = 1;
            foreach (var statement in sortedStatements)
            {
                LogTrace($"\tStatement {iStatement}:");
                if (_Options.Verbose)
                {
                    generator.GenerateScript(statement, out string statementOutput);
                    LogTrace($"\t\t{statementOutput.Trim()}");
                }

                handler.Handle(statement, tree, generator);
                iStatement++;
            }

            var deferredConstraints = tree.GetDeferredConstraints();

            LogInformation($"Beginning Processing of Constraints... {deferredConstraints.Count()}");
            var constraintHandler = provider.GetRequiredService<ISqlServerConstraintHandler>();
            foreach (var constraint in deferredConstraints)
            {
                if (constraintHandler.CanHandle(constraint))
                {
                    if (_Options.Verbose)
                    {
                        generator.GenerateScript(constraint.Constraint, out string constraintOutput);
                        LogTrace($"\t{constraint.Table?.Identifier} => {constraintOutput.Trim()}");
                    }

                    constraintHandler.Handle(constraint, tree, generator);
                }
                else
                {
                    LogWarning($"\tWARNING: Cannot handle constraint of type: {constraint.Constraint?.GetType()?.Name ?? "Unknown"}");
                }
            }

            // NOTE: Tree is built at this point
            ISqlTreeRenderer renderer = provider.GetRequiredService<ISqlTreeRenderer>();

            LogInformation("Beginning Rendering of Sql Models...");

            try
            {
                renderer.FileSaved += OnFileSaved;
                await renderer.RenderAsync(tree);
            } finally
            {
                renderer.FileSaved -= OnFileSaved;
            }
            
            LogInformation("Build Complete!");
        }

        private void OnFileSaved(object sender, FileSavedEventArgs e)
        {
            _Console.Out.Write(e.FilePath + '\n');
        }

        private void LogInformation(string message)
        {
            _Console.Error.Write($"INFO: {message}\n");
        }

        private void LogTrace(string message)
        {
            if (_Options.Verbose)
                _Console.Error.Write($"TRCE: {message}\n");
        }

        private void LogWarning(string message)
        {
            _Console.Error.Write($"WARN: {message}\n");
        }

        private static SqlScriptGenerator CreateSqlGenerator()
        {
            return new Sql150ScriptGenerator(new SqlScriptGeneratorOptions
            {
                KeywordCasing = KeywordCasing.Uppercase,
                SqlEngineType = SqlEngineType.All,
                IncludeSemicolons = true,
            });
        }

        private static IServiceProvider BuildDependencyRoot(BuildSqlCommandOptions commandOptions)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddDefaultFileLoaders<TSqlStatement, DefaultSqlFileStructureLoaderFactory>(commandOptions.Output);
            services.AddTemplateLoader(commandOptions.Template);
            services.AddStubble();
            services.AddSqlServerHandlers(!commandOptions.NoIndexPage);

            return services.BuildServiceProvider();
        }

        private class _BuildSqlCommand { }
    }
}

