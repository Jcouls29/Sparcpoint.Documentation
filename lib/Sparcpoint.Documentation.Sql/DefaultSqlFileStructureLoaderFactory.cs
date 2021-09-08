using Microsoft.SqlServer.TransactSql.ScriptDom;
using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;
using System;
using System.IO;

public class DefaultSqlFileStructureLoaderFactory : IFileStructureLoaderFactory<TSqlStatement>
{
    public IFileStructureLoader FromPath(string path)
    {
        if (Directory.Exists(path))
            return new DirectoryFileStructureLoader(path, "*.sql", new[] { "bin", "obj" });

        throw new ArgumentException("Invalid path supplied.", nameof(path));
    }
}

