using Sparcpoint.Documentation.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Files
{
    public sealed class FlatFileStructureWriter : IFileStructureWriter
    {
        private readonly IFileWriter _Writer;
        private readonly string _RootDirectory;

        public FlatFileStructureWriter(IFileWriter writer, string rootDirectory)
        {
            _Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _RootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
        }

        public async Task WriteAsync<T>(string fileName, byte[] content)
        {
            string path = Path.Combine(_RootDirectory, fileName);
            await _Writer.WriteAsync(path, content);
        }
    }
}
