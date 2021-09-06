using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileStructureWriter
    {
        Task WriteAsync<T>(string name, byte[] content);
    }
}
