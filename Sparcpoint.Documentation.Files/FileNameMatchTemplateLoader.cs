using Sparcpoint.Documentation.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Files
{
    public sealed class FileNameMatchTemplateLoader : ITemplateLoader
    {
        private readonly IFileStructureLoader _Loader;

        public FileNameMatchTemplateLoader(IFileStructureLoaderFactory<Template> loaderFactory, string path)
        {
            _Loader = loaderFactory?.FromPath(path) ?? throw new ArgumentNullException(nameof(loaderFactory));
        }

        public Task<Template?> LoadAsync<TInput>()
        {
            IEnumerable<string> files = _Loader.Load();

            string found = files.FirstOrDefault(f => string.Equals(Path.GetFileNameWithoutExtension(f), typeof(TInput).Name, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(found))
                return Task.FromResult((Template?)new Template
                {
                    Source = found,
                    Value = File.ReadAllText(found)
                });

            return Task.FromResult((Template?)null);
        }
    }
}
