using System.Collections.Generic;
using System.IO;

namespace Sparcpoint.Documentation.Abstractions
{
    public interface IFileStructureLoader
    {
        public IEnumerable<string> Load();
    }
}
