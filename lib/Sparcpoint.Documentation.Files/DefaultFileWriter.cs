using Sparcpoint.Documentation.Abstractions;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Files
{
    public sealed class DefaultFileWriter : IFileWriter
    {
        public Task WriteAsync(string path, byte[] content)
            => System.IO.File.WriteAllBytesAsync(path, content);
    }
}
