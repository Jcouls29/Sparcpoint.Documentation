using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileWriter
    {
        Task Write(string path, byte[] content);
    }
}
