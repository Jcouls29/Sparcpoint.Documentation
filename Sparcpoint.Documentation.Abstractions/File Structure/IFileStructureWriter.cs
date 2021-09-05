using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileStructureWriter
    {
        Task Write<T>(string fileName, byte[] content);
    }
}
