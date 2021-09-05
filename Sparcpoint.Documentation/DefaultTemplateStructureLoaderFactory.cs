// See https://aka.ms/new-console-template for more information

using Sparcpoint.Documentation.Abstractions;
using Sparcpoint.Documentation.Files;
using System.Text;

public class DefaultTemplateStructureLoaderFactory : IFileStructureLoaderFactory<Template>
{
    public IFileStructureLoader FromPath(string path)
    {
        if (Directory.Exists(path))
            return new DirectoryFileStructureLoader(path, "*.template");

        throw new ArgumentException("Invalid path supplied.", nameof(path));
    }
}

public class TextFileWriter : IFileWriter
{
    public Task Write(string path, byte[] content)
        => File.WriteAllTextAsync(path, Encoding.UTF8.GetString(content));
}

