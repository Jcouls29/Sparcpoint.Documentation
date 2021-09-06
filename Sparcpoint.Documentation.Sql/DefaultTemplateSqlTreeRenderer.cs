using Sparcpoint.Documentation.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await RenderAsync(tree.Sequences);
            await RenderAsync(tree.Views);
            await RenderAsync(tree.StoredProcedures);

            await RenderIndexAsync(tree);
        }

        private async Task RenderIndexAsync(IReadOnlySqlTree tree)
        {
            Template template = await _TemplateLoader.LoadAsync<Index>();
            if (template != null)
            {
                string output = await _TemplateProcessor.ProcessAsync(template, new Index
                {
                    Schemas = tree.Schemas,
                    Tables = tree.Tables,
                    TableTypes = tree.TableTypes,
                    Views = tree.Views,
                    StoredProcedures = tree.StoredProcedures,
                    Sequences = tree.Sequences
                });
                await _FileWriter.WriteAsync<Index>($"Index{template.FileExtension}", Encoding.UTF8.GetBytes(output));
            }
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

        private class Index
        {
            public IEnumerable<SchemaModel> Schemas { get; set; }
            public bool HasSchemas => Schemas?.Any() ?? false;

            public IEnumerable<TableModel> Tables { get; set; }
            public bool HasTables => Tables?.Any() ?? false;

            public IEnumerable<TableTypeModel> TableTypes { get; set; }
            public bool HasTableTypes => TableTypes?.Any() ?? false;

            public IEnumerable<ViewModel> Views { get; set; }
            public bool HasViews => Views?.Any() ?? false;

            public IEnumerable<StoredProcedureModel> StoredProcedures { get; set; }
            public bool HasStoredProcedures => StoredProcedures?.Any() ?? false;

            public IEnumerable<SequenceModel> Sequences { get; set; }
            public bool HasSequences => Sequences?.Any() ?? false;
        }
    }
}
