using Sparcpoint.Documentation.Abstractions;
using System;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public sealed class SqlServerFileWriter : IFileStructureWriter
    {
        private readonly IFileWriter _Writer;
        private readonly string _RootDirectory;

        public SqlServerFileWriter(IFileWriter writer, string rootDirectory)
        {
            _Writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _RootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
        }

        public Task Write<T>(string name, byte[] content)
        {
            throw new NotImplementedException();
        }
    }
}
