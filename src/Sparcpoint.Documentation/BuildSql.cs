using Sparcpoint.Documentation.BuildSql;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Reflection;

public static class BuildSql
{
    public static Command GetCommand()
    {
        var buildCommand = new Command("build-sql", "Build the SQL Commands into Markdown")
        {
            new Option<string>(new[] { "-t", "--template" }, GetDefaultTemplatePath, "Path to the template directory"),
            new Option<bool>("--no-index-page", "Do not generate an index page"),
            new Option<bool>(new[] { "-v", "--verbose" }, "Show detail output logging"),
            new Argument<string>("path", "Path to source directory or .dbproj"),
            new Argument<string>("output", "Path to output directory"),
        };

        buildCommand.Handler = CommandHandler.Create(async (BuildSqlCommandOptions options, IConsole console) =>
        {
            var command = new BuildSqlCommand(options, console);
            await command.Run();
        });
        return buildCommand;
    }

    private static string GetDefaultTemplatePath()
    {
        string? directory = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location ?? throw new Exception("Invalid entry assembly. Be sure to access this method from an appropriate EXE."));
        return Path.Combine(directory ?? @".\", @"Templates\Default");
    }
}

