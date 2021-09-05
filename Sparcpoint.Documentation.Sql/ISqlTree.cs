using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Linq;

namespace Sparcpoint.Documentation.Sql
{
    public interface ISqlTree
    {
        void Add<T>(T model) where T : ISqlModel;
        T Get<T>(SqlIdentifier identifier) where T : ISqlModel;
        IEnumerable<T> Get<T>() where T : ISqlModel;

        void DeferConstraint(TableModel table, ConstraintDefinition definition);
        IEnumerable<DeferredTableConstraint> GetDeferredConstraints();

        IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        IReadOnlySqlIndexer<TableModel> Tables { get; }
        IReadOnlySqlIndexer<TableTypeModel> TableTypes { get; }
    }

    public sealed class InMemorySqlTree : ISqlTree
    {
        private readonly List<ISqlModel> _Models = new List<ISqlModel>();
        private readonly List<DeferredTableConstraint> _DeferredConstraints = new List<DeferredTableConstraint>();

        public InMemorySqlTree()
        {
            Schemas = new DefaultReadOnlySqlIndexer<SchemaModel>(this);
            Tables = new DefaultReadOnlySqlIndexer<TableModel>(this);
            TableTypes = new DefaultReadOnlySqlIndexer<TableTypeModel>(this);

            _Models.Add(new SchemaModel
            {
                Identifier = new SqlIdentifier("dbo"),
            });
        }

        public IReadOnlySqlIndexer<SchemaModel> Schemas { get; }
        public IReadOnlySqlIndexer<TableModel> Tables { get; }
        public IReadOnlySqlIndexer<TableTypeModel> TableTypes { get; }

        public void Add<T>(T model) where T : ISqlModel
        {
            // TODO: Check for model already added.

            _Models.Add(model);
        }

        public T Get<T>(SqlIdentifier identifier) where T : ISqlModel
            => (T)_Models.FirstOrDefault(m => m is T && m.Identifier == identifier);

        public IEnumerable<T> Get<T>() where T : ISqlModel
            => _Models.Where(m => m is T).Select(m => (T)m).ToArray();

        public void DeferConstraint(TableModel table, ConstraintDefinition definition)
            => _DeferredConstraints.Add(new DeferredTableConstraint { Table = table, Constraint = definition });
        public IEnumerable<DeferredTableConstraint> GetDeferredConstraints()
            => _DeferredConstraints;
    }

    public sealed class DeferredTableConstraint
    {
        public TableModel Table { get; set; }
        public ConstraintDefinition Constraint { get; set; }
    }
}
