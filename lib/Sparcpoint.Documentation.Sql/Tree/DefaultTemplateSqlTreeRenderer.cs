using Microsoft.Extensions.Options;
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
        private readonly IOptionsMonitor<TreeRendererOptions> _Options;

        public event EventHandler<FileSavedEventArgs> FileSaved;
        protected void OnFileSaved(string path)
            => FileSaved?.Invoke(this, new FileSavedEventArgs { FilePath = path });

        public DefaultTemplateSqlTreeRenderer(
            ITemplateLoader templateLoader, 
            ITemplateProcessor<string> templateProcessor, 
            IFileStructureWriter fileWriter,
            IOptionsMonitor<TreeRendererOptions> options)
        {
            _TemplateLoader = templateLoader ?? throw new ArgumentNullException(nameof(templateLoader));
            _TemplateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
            _FileWriter = fileWriter ?? throw new ArgumentNullException(nameof(fileWriter));
            _Options = options ?? throw new ArgumentNullException(nameof(options));

            _FileWriter.FileSaved += (sender, e) => OnFileSaved(e.FilePath);
        }

        public async Task RenderAsync(IReadOnlySqlTree tree)
        {
            await RenderAsync(tree.Schemas);
            await RenderAsync(tree.Tables);
            await RenderAsync(tree.TableTypes);
            await RenderAsync(tree.Sequences);
            await RenderAsync(tree.Views);
            await RenderAsync(tree.StoredProcedures);
            await RenderAsync(tree.Functions);
            await RenderAsync(tree.DataTypes);

            if (_Options.CurrentValue?.RenderIndex ?? true)
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
                    Sequences = tree.Sequences,
                    Functions = tree.Functions,
                    DataTypes = tree.DataTypes,
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

            public IEnumerable<FunctionModel> Functions { get; set; }
            public bool HasFunctions => Functions?.Any() ?? false;

            public IEnumerable<DataTypeModel> DataTypes { get; set; }
            public bool HasDataTypes => DataTypes?.Any() ?? false;
        }
    }
}
