using System.CommandLine;

var rootCommand = new RootCommand
{
    BuildSqlCommand.GetCommand()
};

await rootCommand.InvokeAsync(args);
