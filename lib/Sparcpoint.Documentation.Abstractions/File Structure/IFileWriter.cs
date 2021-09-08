using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileWriter
    {
        Task WriteAsync(string path, byte[] content);
    }
}
