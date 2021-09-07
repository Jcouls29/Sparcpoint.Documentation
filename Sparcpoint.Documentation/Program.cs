using System.CommandLine;
using System.Threading.Tasks;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand
        {
            BuildSqlCommand.GetCommand()
        };

        await rootCommand.InvokeAsync(args);
    }
} 

