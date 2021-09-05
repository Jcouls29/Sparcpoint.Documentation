using Sparcpoint.Documentation.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sparcpoint.Documentation.Files
{
    public sealed class DirectoryFileStructureLoader : IFileStructureLoader
    {
        private readonly string _Path;
        private string _Pattern;
        private readonly IEnumerable<string> _ExceptFolderNames;

        public DirectoryFileStructureLoader(string path, string pattern, IEnumerable<string> exceptFolderNames = null)
        {
            _Path = path ?? throw new ArgumentNullException(nameof(path));
            _Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            _ExceptFolderNames = exceptFolderNames;
        }

        public IEnumerable<string> Load()
        {
            IEnumerable<string> allFiles = Directory.EnumerateFiles(_Path, _Pattern, SearchOption.AllDirectories);
            if (_ExceptFolderNames?.Any() ?? false)
                allFiles = allFiles
                    .Where(f => !_ExceptFolderNames.Any(folderName => f.Contains(@$"\{folderName}\")))
                    .ToArray();

            return allFiles;
        }
    }
}
