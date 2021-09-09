using System.Collections.Generic;
using System.IO;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileStructureLoader
    {
        IEnumerable<string> Load();
    }
}
