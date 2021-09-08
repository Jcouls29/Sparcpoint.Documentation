using System;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileStructureWriter
    {
        event EventHandler<FileSavedEventArgs> FileSaved;

        Task WriteAsync<T>(string name, byte[] content);
    }

    public class FileSavedEventArgs : EventArgs
    {
        public string FilePath { get; set; }
    }
}
