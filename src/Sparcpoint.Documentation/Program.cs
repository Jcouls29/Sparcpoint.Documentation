using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

public static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            BuildSql.GetCommand()
        };

        var parser = new CommandLineBuilder(rootCommand)
            .UseDefaults()
            .UseExceptionHandler((ex, context) =>
            {
                context.Console.Error.Write($"ERR: {ex.Message}");
                context.ExitCode = 1;
            })
            .Build();

        return await rootCommand.InvokeAsync(args);
    }
} 

