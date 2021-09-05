using System.Collections.Generic;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlTree
    {
        void Add<T>(T model) where T : ISqlModel;
        T Get<T>(SqlIdentifier identifier) where T : ISqlModel;

        IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        IReadOnlySqlIndexer<TableModel> Tables { get; }
    }

    public sealed class InMemorySqlTree : ISqlTree
    {
        private readonly List<ISqlModel> _Models = new List<ISqlModel>();

        public InMemorySqlTree()
        {
            Schemas = new DefaultReadOnlySqlIndexer<SchemaModel>(this);
            Tables = new DefaultReadOnlySqlIndexer<TableModel>(this);
        }

        public IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        public IReadOnlySqlIndexer<TableModel> Tables { get; }

        public void Add<T>(T model) where T : ISqlModel
        {
            // TODO: Check for model already added.

            _Models.Add(model);
        }

        public T Get<T>(SqlIdentifier identifier) where T : ISqlModel
            => (T)_Models.FirstOrDefault(m => m is T && m.Identifier == identifier);
    }
}
