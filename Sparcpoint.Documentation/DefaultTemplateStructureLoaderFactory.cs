// See https://aka.ms/new-console-template for more information

using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;
using System.Text;

public class DefaultTemplateStructureLoaderFactory : IFileStructureLoaderFactory<Template>
{
    public IFileStructureLoader FromPath(string path)
    {
        if (Directory.Exists(path))
            return new DirectoryFileStructureLoader(path, "*.*");

        throw new ArgumentException("Invalid path supplied.", nameof(path));
    }
}

