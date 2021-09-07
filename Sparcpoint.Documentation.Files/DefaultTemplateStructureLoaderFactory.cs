using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;
using System;
using System.IO;

public class DefaultTemplateStructureLoaderFactory : IFileStructureLoaderFactory<Template>
{
    public IFileStructureLoader FromPath(string path)
    {
        if (Directory.Exists(path))
            return new DirectoryFileStructureLoader(path, "*.*");

        throw new ArgumentException("Invalid path supplied.", nameof(path));
    }
}

