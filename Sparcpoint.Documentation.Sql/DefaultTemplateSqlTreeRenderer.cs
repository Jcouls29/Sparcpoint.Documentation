using Sparcpoint.Documentation.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sparcpoint.Documentation.Sql
{
    public class DefaultTemplateSqlTreeRenderer : ISqlTreeRenderer
    {
        private readonly ITemplateLoader _TemplateLoader;
        private readonly ITemplateProcessor<string> _TemplateProcessor;
        private readonly IFileStructureWriter _FileWriter;

        public DefaultTemplateSqlTreeRenderer(
            ITemplateLoader templateLoader, 
            ITemplateProcessor<string> templateProcessor, 
            IFileStructureWriter fileWriter)
        {
            _TemplateLoader = templateLoader ?? throw new ArgumentNullException(nameof(templateLoader));
            _TemplateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _FileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
        }

        public async Task RenderAsync(IReadOnlySqlTree tree)
        {
            await RenderAsync(tree.Schemas);
            await RenderAsync(tree.Tables);
            await RenderAsync(tree.TableTypes);
        }

        private async Task RenderAsync<T>(IEnumerable<T> models)
            where T : ISqlModel
        {
            Template template = await _TemplateLoader.LoadAsync<T>();
            if (template != null)
                foreach (var model in models)
                {
                    string output = await _TemplateProcessor.ProcessAsync(template, model);
                    await _FileWriter.WriteAsync<T>($"{model.Identifier}{template.FileExtension}", Encoding.UTF8.GetBytes(output));
                }
        }
    }
}
