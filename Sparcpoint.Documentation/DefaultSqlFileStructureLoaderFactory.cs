// See https://aka.ms/new-console-template for more information

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;

public class DefaultSqlFileStructureLoaderFactory : IFileStructureLoaderFactory<TSqlStatement>
{
    public IFileStructureLoader FromPath(string path)
    {
        if (Directory.Exists(path))
            return new DirectoryFileStructureLoader(path, "*.sql", new[] { "bin", "obj" });

        throw new ArgumentException("Invalid path supplied.", nameof(path));
    }
}

